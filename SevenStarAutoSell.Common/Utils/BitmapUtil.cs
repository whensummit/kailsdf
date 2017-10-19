using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace SevenStarAutoSell.Common.Utils
{
    public static class BitmapUtil
    {
        /// <summary>
        /// 图像尺寸修改
        /// </summary>
        /// <param name="image">原图</param>
        /// <param name="maxWidth">放大图片的最大宽度</param>
        /// <param name="cutStartX">切掉左侧的像素值（使得切割以后的对称）</param>
        /// <param name="needPadding">是否要加内边距（默认不加）</param>
        /// <returns></returns>
        public static void ResizeImage(ref Bitmap image, int maxWidth, int cutStartX, bool needPadding)
        {
            if (image.Width > maxWidth)
            {
                return;
            }

            double iSzie = 300.00 / image.Width;
            int newWidth = (int)(iSzie * image.Width);
            int newHeight = (int)(iSzie * image.Height);

            using (var destImage = new Bitmap(newWidth, newHeight))
            using (var graphics = Graphics.FromImage(destImage))
            using (var wrapMode = new ImageAttributes())
            {
                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var destRect = new Rectangle(0, 0, newWidth, newHeight);
                if (needPadding)
                {
                    // 先用原图左上角的样本像素填充底色
                    graphics.DrawImage(image, destRect, new Rectangle(0, 0, 1, 1), GraphicsUnit.Pixel);

                    // 把原图片绘入底图（内边距留10像素）
                    int padding = 10;
                    graphics.DrawImage(image,
                        new Rectangle(padding, padding, newWidth - padding * 2, newHeight - padding * 2),
                        cutStartX, 0, image.Width - cutStartX, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
                else
                {
                    graphics.DrawImage(image, destRect, cutStartX, 0, image.Width - cutStartX, image.Height,
                        GraphicsUnit.Pixel, wrapMode);
                }

                image = new Bitmap(destImage);
            }
        }

        /// <summary>
        /// 图片灰度处理
        /// </summary>
        /// <param name="original">原图片</param>
        /// <returns></returns>
        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //make an empty bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = original.GetPixel(i, j);

                    //create the grayscale version of the pixel
                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                                           + (originalColor.B * .11));

                    //create the color object
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    //set the new image's pixel to the grayscale version
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }

        /// <summary>
        /// bitmap转byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] Bitmap2Bytes(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        /// <summary>
        /// 切割图片
        /// </summary>
        /// <param name="fromImage"></param>
        /// <param name="FromImagePointX"></param>
        /// <param name="FromImagePointY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image CutImage(Image fromImage, int FromImagePointX, int FromImagePointY, int width, int height)
        {
            //创建新图位图   
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域   
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区   
            graphic.DrawImage(fromImage, FromImagePointX, FromImagePointY, new Rectangle(0, 0, fromImage.Width, fromImage.Height), GraphicsUnit.Point);
            //从作图区生成新图   
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //释放资源   
            bitmap.Dispose();
            graphic.Dispose();
            return saveImage;
        }


        #region 图片处理

        /// <summary>
        /// 置灰白
        /// </summary>
        /// <param name="img"></param>
        public static Bitmap BlackAndWhite(Image img)
        {
            Bitmap wew32 = new Bitmap(img);
            Bitmap newBitmap = new Bitmap(wew32.Width, wew32.Height);
            Color grayColor = Color.FromArgb(200, 200, 200);
            for (int i = 0; i < wew32.Width; i++)
            {
                for (int j = 0; j < wew32.Height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = wew32.GetPixel(i, j);

                    //create the grayscale version of the pixel
                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                                           + (originalColor.B * .11));

                    //create the color object
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    if (JudgeAroundIsGrayColor(wew32, i, j, grayColor))
                        newBitmap.SetPixel(i, j, grayColor);
                    else
                        //set the new image's pixel to the grayscale version
                        newBitmap.SetPixel(i, j, newColor);
                }
            }
            return newBitmap;
        }

        /// <summary>
        /// 判断该点四周颜色是否为灰,目的是 驱除图片的痘痘
        /// </summary>
        /// <param name="originalMap"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="grayColor"></param>
        /// <returns></returns>
        public static bool JudgeAroundIsGrayColor(Bitmap originalMap, int i, int j, Color grayColor)
        {
            if (i < 1 || j < 1 || i >= originalMap.Width - 1 || j >= originalMap.Height - 1) return true;
            int account = 0;
            if (IsGrayColor(originalMap, i + 1, j, grayColor)) account++;
            if (IsGrayColor(originalMap, i - 1, j, grayColor)) account++;
            if (IsGrayColor(originalMap, i, j - 1, grayColor)) account++;
            if (IsGrayColor(originalMap, i, j + 1, grayColor)) account++;
            if (account >= 3) return true;
            return false;
        }

        /// <summary>
        /// 判断该点颜色是否为灰
        /// </summary>
        /// <param name="originalMap"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="grayColor"></param>
        /// <returns></returns>
        public static bool IsGrayColor(Bitmap originalMap, int i, int j, Color grayColor)
        {
            return grayColor.Equals(originalMap.GetPixel(i, j));
        }

        #endregion

        #region 汉字识别库

        /// <summary>
        /// 汉子识别字体库
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static int Character(string original)
        {
            original = original.Trim();
            if (original.IndexOf("\n", StringComparison.Ordinal) != -1) original = original.Replace("\n", "");
            switch (original)
            {
                case "1":
                case "一":
                case "′-":
                case "壹":
                case "叠":
                case "囊":
                case "蠢":
                case "′":
                case "/":
                case "'":
                case "′一":
                case "】":
                case "薹":
                    return 1;
                case "2":
                case "二":
                case "z":
                case "Z":
                case "贰":
                case "贰】":
                case "′′_":
                case "′_":
                case "甙":

                    return 2;
                case "3":
                case "三":
                case ";一":
                case "叁":
                case "参":
                case "叁-":
                case "叁)":
                case "易":
                case "易'":
                case "基":
                case "夏-":
                case "袁-":
                case "袁":
                case "荤":
                case "荤-":
                case "蓥":
                case "晶":
                case "塞":
                case "鲁":
                case "曼`":
                case "旨":
                case "曼.":
                case "拳":
                case "墨":
                case "叁、":
                case "鼻":
                case "巷":
                case "荃":
                case "畜":
                case "羹":
                case "答":
                case "茗、":
                case "茗":
                case "基`":
                case "奚":
                case "奚`":
                case "厦-":
                case "厦":
                case "哀-":
                case "哀":
                case "s":
                case "S":
                case "妾":
                case "堇":
                case "重-":
                case "重":
                case "秦":
                case "翼":
                case "藿、":
                case "藿":
                case "抖`":
                case "抖":
                case "异":
                case "禹":
                case "多":
                    return 3;
                case "4":
                case "四":
                case "肆":
                case "匪】":
                case "田":
                case "叹":
                case "讽":
                case "薯":
                case "戍":
                case "姆":
                case "廖】":
                case "嫖":
                case "暮":
                case "熹":
                case "砜":
                case "凹":
                case "西":
                case "昭":
                case "明":
                case "虞":
                case "蜱":
                case "阂":
                case "曰":
                case "婉":
                case "壕":
                    return 4;
                case "5":
                case "五":
                case "伍":
                case "桩":
                case "板":
                case "橇":
                case "堑":
                case "罩":
                case "堑-":
                case "荚":
                case "蚤-":
                case "蚤":
                case "亟-":
                case "亟":
                case "梗、":
                case "梗":
                case "珥-":
                case "珥":
                case "趸":
                case "枉":
                case "荬":
                case "玉":
                    return 5;
                case "6":
                case "六":
                case "陆":
                case "垛`′、":
                case "}'′、":
                case "甙`":
                case "圃":
                case "荞":
                case "隙":
                case "胺":
                case "睹":
                    return 6;
                case "7":
                case "七":
                case "柒":
                case "束":
                case "省":
                case "棠":
                case "渠":
                case "耒":
                case "桑":
                case "秉":
                case "蕹":
                case "扫":
                case "匕":
                case "-杠":
                case "七`":
                    return 7;
                case "8":
                case "八":
                case "捌":
                case "髯":
                case "删":
                case "〖":
                case "/\\":
                case "轻":
                case "制":
                case "黏":
                case "堀":
                case "槲":
                case "蜜":
                case "墩":
                case "涮":
                case "E":
                case "测":
                case "蟹":
                case "雇":
                case "g":
                    return 8;
                case "9":
                case "九":
                case "玖":
                case "次":
                case "?":
                case "镜":
                case "班":
                case "殊":
                case "沁":
                case "今":
                case "叟":
                case "碾":
                case "燮":
                case "覆":
                case "仔":
                case "矾":
                case "疯":
                case "孽":
                case "姝":
                case "汐、":
                case "汐":
                case "允":
                case "郝":
                case "蛛":
                    return 9;
                case "0":
                case "零":
                case "〇":
                case "皋":
                case "霖":
                case "寒":
                case "o":
                case "灞)":
                case "灞，":
                case "灞":
                case "寨":
                case "暴":
                case "幂":
                case "聚)":
                case "聚":
                case "琴":
                case "。":
                case "蓁":
                    return 0;
            }
            return 2;
        }

        #endregion
    }
}
