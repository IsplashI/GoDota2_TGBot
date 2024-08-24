using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GoDota2_Bot
{
    public static class ScreenCapture
    {
        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
            IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        public static string fileName = "screenshot.png";
        public static void CaptureScreen()
        {
            
            try
            {
                // Get the size of the primary screen
                int screenWidth = GetSystemMetrics(0);  // SM_CXSCREEN = 0
                int screenHeight = GetSystemMetrics(1); // SM_CYSCREEN = 1

                // Get the device context of the entire screen
                IntPtr desktopHandle = GetDesktopWindow();
                IntPtr desktopDC = GetWindowDC(desktopHandle);

                // Create a compatible device context and bitmap
                IntPtr memoryDC = CreateCompatibleDC(desktopDC);
                IntPtr bitmap = CreateCompatibleBitmap(desktopDC, screenWidth, screenHeight);
                IntPtr oldBitmap = SelectObject(memoryDC, bitmap);

                // Copy the screen into the bitmap
                BitBlt(memoryDC, 0, 0, screenWidth, screenHeight, desktopDC, 0, 0, CopyPixelOperation.SourceCopy);

                // Create a .NET bitmap from the HBITMAP
                Bitmap result = Image.FromHbitmap(bitmap);

                // Restore the old bitmap and clean up
                SelectObject(memoryDC, oldBitmap);
                DeleteObject(bitmap);
                DeleteDC(memoryDC);
                ReleaseDC(desktopHandle, desktopDC);

                // Save the bitmap to a file
                
                result.Save(fileName, ImageFormat.Png);

                Console.WriteLine($"Screenshot saved as {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);
    }
}
