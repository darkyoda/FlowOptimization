using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FlowOptimization.Data.Pipeline;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FlowOptimization.Utilities.Canvas
{
    /// <summary>
    /// Класс для графического отображения элементов на glControl
    /// </summary>
    static class DrawingUtilities
    {
        public static int RectSide { get; set; }

        private static readonly SpriteFont Font = new SpriteFont(new Font("Consolas", 15)); // Шрифт

        static DrawingUtilities()
        {
            RectSide = 30;
        }

        /// <summary>
        /// Графическое отображение узла
        /// </summary>
        /// <param name="node">Объект узла</param>
        public static void DrawRect(Node node)
        {
            if (node.NodeType == Node.Type.Enter)
                GL.Color4(Color.Blue);
            else if (node.NodeType == Node.Type.Exit || node.Volume != 0)
                GL.Color4(Color.DarkOrange);
            else if (node.NodeType == Node.Type.Default)
                GL.Color4(Color.Green);

            GL.Begin(BeginMode.Quads);
            GL.Vertex2(node.X, node.Y);
            GL.Vertex2(node.X + RectSide, node.Y);
            GL.Vertex2(node.X + RectSide, node.Y + RectSide);
            GL.Vertex2(node.X, node.Y + RectSide);
            GL.End();

            DrawString(Font, node.X, node.Y + RectSide - 3, 1f, node.ID.ToString(), Color.White);
            if (node.Volume != 0)
                DrawString(Font, node.X + 5, node.Y + RectSide + RectSide * 1/2, 0.6f, node.Volume.ToString(), Color.White);
        }

        /// <summary>
        /// Графическое отображение рамки
        /// </summary>
        /// <param name="node">Объект выделенного узла</param>
        public static void DrawGrid(Node node)
        {
            GL.Color4(Color.Yellow);
            GL.Begin(BeginMode.Lines); // try PolygonMode
            GL.LineWidth(1f);
            GL.Vertex2(node.X - 1, node.Y - 1);
            GL.Vertex2(node.X + 1 + RectSide, node.Y - 1);
            GL.Vertex2(node.X + 1 + RectSide, node.Y + 1 + RectSide);
            GL.Vertex2(node.X - 1, node.Y + 1 + RectSide);
            GL.Vertex2(node.X + 1 + RectSide, node.Y - 1);
            GL.Vertex2(node.X + 1 + RectSide, node.Y + 1 + RectSide);
            GL.Vertex2(node.X - 1, node.Y + 1 + RectSide);
            GL.Vertex2(node.X - 1, node.Y - 1);
            GL.End();
        }

        /// <summary>
        /// Графическое отображение связи при её перетягивании мышью
        /// </summary>
        /// <param name="node">Объект узла из которого он создан</param>
        /// <param name="x">Координаты мыши по X</param>
        /// <param name="y">Координаты мыши по Y</param>
        public static void DrawLineByMouse(Node node, int x, int y)
        {
            GL.LineWidth(3f);
            GL.Color4(Color.Red);
            GL.Begin(BeginMode.Lines);
            GL.Vertex2((node.X + RectSide / 2), (node.Y + RectSide / 2));   // Центр квадрата
            GL.Vertex2(x, y);
            GL.End();

        }

        /// <summary>
        /// Графическое отображение связи
        /// </summary>
        /// <param name="pipe">Объект связи</param>
        public static void DrawLine(Pipe pipe)
        {
            GL.LineWidth(1.5f);
            GL.Color4(Color.Black);
            GL.Begin(BeginMode.Lines);
            GL.Vertex2((pipe.StartNode.X + RectSide / 2), (pipe.StartNode.Y + RectSide / 2));   // Центр квадрата
            GL.Vertex2((pipe.EndNode.X + RectSide / 2), (pipe.EndNode.Y + RectSide / 2));
            GL.End();

            DrawString(Font, pipe.StartNode.X - ((pipe.StartNode.X - pipe.EndNode.X) / 2), pipe.StartNode.Y - ((pipe.StartNode.Y - pipe.EndNode.Y) / 2), 0.6f, pipe.Length.ToString(), Color.Black);
        }

        /// <summary>
        /// Загрузка текстуры подложки
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <param name="generateMipMaps">Генерировать ли MipMaps</param>
        /// <returns>Возвращает текстуру подложки, если такая существует</returns>
        public static TextureInfo LoadTexture(string filename, bool generateMipMaps = true)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            return LoadTexture(new Bitmap(filename), generateMipMaps);
        }

        /// <summary>
        /// Загрузка текстуры шрифтов
        /// </summary>
        /// <param name="bmp">Файл шрифтов</param>
        /// <param name="generateMipMaps">Генерировать ли MipMaps</param>
        /// <returns></returns>
        public static TextureInfo LoadTexture(Bitmap bmp, bool generateMipMaps = true)
        {
            var id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (generateMipMaps)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                //GL.Ext.GenerateTextureMipmap(ID, TextureTarget.Texture2D);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return new TextureInfo { GlId = id, Height = bmp.Height, Width = bmp.Width };
        }

        /// <summary>
        /// Нарисовать текст
        /// </summary>
        /// <param name="font">Шрифт</param>
        /// <param name="x">Координаты по X относительно glControl</param>
        /// <param name="y">Координаты по Y относительно glControl</param>
        /// <param name="scale">Размер</param>
        /// <param name="text">Текст</param>
        /// <param name="color">Цвет шрифта</param>
        public static void DrawString(SpriteFont font, int x, int y, float scale, string text, Color color)
        {
            if(font == null) return;
            GL.BindTexture(TextureTarget.Texture2D,font.GlTextureId);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);
                    
            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var coords = font.Get(ch);

                //  Top Left
                GL.TexCoord2(coords.X1, coords.Y1);
                GL.Vertex2(x + (i * font.CharWidth + i * font.Kern) * scale, y);

                // Top Right
                GL.TexCoord2(coords.X2, coords.Y1);
                GL.Vertex2(x + ((i + 1) * font.CharWidth + i * font.Kern) * scale, y);

                // Bottom Right
                GL.TexCoord2(coords.X2, coords.Y2);
                GL.Vertex2(x + ((i + 1) * font.CharWidth + i * font.Kern) * scale, y - font.CharHeight * scale);

                // Bottom Left
                GL.TexCoord2(coords.X1, coords.Y2);
                GL.Vertex2(x + (i * font.CharWidth + i * font.Kern) * scale, y - font.CharHeight * scale);
            }
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Рисуем текстуру
        /// </summary>
        /// <param name="texture">Текстура</param>
        /// <param name="x">Координаты по X</param>
        /// <param name="y">Координаты по Y</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        public static void DrawTexture(TextureInfo texture, float x, float y, float scale)
        {
            GL.BindTexture(TextureTarget.Texture2D, texture.GlId);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color.White);

            // Top Left
            GL.TexCoord2(0, 0);
            GL.Vertex2(x, y);

            // Top Right
            GL.TexCoord2(1, 0);
            GL.Vertex2(x + texture.Width * scale, y);

            // Bottom Right
            GL.TexCoord2(1, 1);
            GL.Vertex2(x + texture.Width * scale, y - texture.Height * scale);

            // Bottom Left
            GL.TexCoord2(0, 1);
            GL.Vertex2(x, y - texture.Height * scale);

            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
