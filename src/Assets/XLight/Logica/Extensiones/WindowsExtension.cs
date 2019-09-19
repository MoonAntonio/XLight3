//                                  ┌∩┐(◣_◢)┌∩┐
//                                                                              \\
// WindowsExtension.cs (19/09/2019)                                             \\
// Autor: Antonio Mateo (.\Moon Antonio)    antoniomt.moon@gmail.com            \\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using System;
using System.Runtime.InteropServices;
#endregion

namespace XLight.Windows
{
    /// <summary>
    /// <para>Extension de <see cref="WindowsController"/>. Contiene funcionalidad
    /// que es sacada de la dll de user32.dll en el SO Windows 10.</para>
    /// </summary>
    public class WindowsExtension
	{
        #region DLL
        /// <summary>
        /// <para>Obtiene la ventana activa actualmente.</para>
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
        /// <summary>
        /// <para>Determina el largo de window.</para>
        /// </summary>
        /// <param name="hWnd">La ventana.</param>
        /// <param name="nIndex">Desplazamiento inicial.</param>
        /// <param name="dwNewLong">Largo de la ventana.</param>
        /// <returns></returns>
        [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        /// <summary>
        /// <para>Establece el estado de visualizacion de la ventana especificada.</para>
        /// </summary>
        /// <param name="hwnd">La ventana.</param>
        /// <param name="nCmdShow">Controla como se debe mostrar la ventana.</param>
        /// <returns></returns>
        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        /// <summary>
        /// <para>Cambia la posicion y las dimensiones de la ventana especificada. Para una ventana de nivel superior, 
        /// la posicion y las dimensiones son relativas a la esquina superior izquierda de la pantalla. Para una ventana secundaria, 
        /// son relativas a la esquina superior izquierda del area del cliente de la ventana primaria.</para>
        /// </summary>
        /// <param name="hWnd">La ventana.</param>
        /// <param name="x">La nueva posicion del lado izquierdo de la ventana.</param>
        /// <param name="y">La nueva posicion de la parte superior de la ventana.</param>
        /// <param name="nWidth">El nuevo ancho de la ventana.</param>
        /// <param name="nHeight">La nueva altura de la ventana.</param>
        /// <param name="bRepaint">Indica si la ventana se debe volver a pintar.</param>
        /// <returns></returns>
        [DllImport("user32.dll")] private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        /// <summary>
        /// <para>Recupera las dimensiones del rectangulo delimitador de la ventana especificada. 
        /// Las dimensiones se dan en coordenadas de pantalla que son relativas a la esquina superior izquierda de la pantalla.</para>
        /// </summary>
        /// <param name="hwnd">La ventana.</param>
        /// <param name="lpRect">Un puntero a una estructura RECT que recibe las coordenadas de la pantalla de las esquinas superior izquierda e inferior derecha de la ventana.</param>
        /// <returns></returns>
        [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr hwnd, out WinRect lpRect);
        #endregion

        #region Constantes
        private const int GWL_ESTILO = -16;
        private const int SW_MINIMIZAR = 6;
        private const int SW_MAXIMIZAR = 3;
        private const int SW_RESTAURAR = 9;
        private const uint WS_VISIBLE = 0x10000000;
        private const uint WS_POPUP = 0x80000000;
        private const uint WS_SUPERPUESTO = 0x00000000;
        private const uint WS_ENCABEZADO = 0x00C00000;
        private const uint WS_SYSMENU = 0x00080000;
        private const uint WS_GRUESOMARCO = 0x00040000;
        private const uint WS_CAJAMINIMIZADA = 0x00020000;
        private const uint WS_CAJAMAXIMIZADA = 0x00010000;
        private const uint WS_WINDOWSUPERPUESTA = WS_SUPERPUESTO | WS_ENCABEZADO | WS_SYSMENU | WS_GRUESOMARCO | WS_CAJAMINIMIZADA | WS_CAJAMAXIMIZADA;
        #endregion

        #region Variables Estaticas
        /// <summary>
        /// <para>Determina si el marco/borde esta activado o no.</para>
        /// </summary>
        public static bool isMarco = true;
        #endregion

        #region Inicializadores
        /// <summary>
        /// <para>Inicializador de <see cref="WindowsExtension"/>.Este atributo hara que el metodo se ejecute en el lanzamiento del juego.</para>
        /// </summary>
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeOnLoad()
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN   // Mientas se esta en el Editor, no ejecutar esto
            SetWindowSinMarco();
#endif
        }
        #endregion

        #region API
        /// <summary>
        /// <para>Determina las medidas de la ventana sin marco.</para>
        /// </summary>
        public static void SetWindowSinMarco()
        {
            var hwnd = GetActiveWindow();
            SetWindowLong(hwnd, GWL_ESTILO, WS_POPUP | WS_VISIBLE);
            isMarco = false;
        }

        /// <summary>
        /// <para>Determina las medidas de la ventana con marco.</para>
        /// </summary>
        public static void SetWindowConMarco()
        {
            var hwnd = GetActiveWindow();
            SetWindowLong(hwnd, GWL_ESTILO, WS_WINDOWSUPERPUESTA | WS_VISIBLE);
            isMarco = true;
        }

        /// <summary>
        /// <para>Minimiza la ventana.</para>
        /// </summary>
        public static void MinimizarWindow()
        {
            var hwnd = GetActiveWindow();
            ShowWindow(hwnd, SW_MINIMIZAR);
        }

        /// <summary>
        /// <para>Maximiza la ventana.</para>
        /// </summary>
        public static void MaximizarWindow()
        {
            var hwnd = GetActiveWindow();
            ShowWindow(hwnd, SW_MAXIMIZAR);
        }

        /// <summary>
        /// <para>Restaura la ventana.</para>
        /// </summary>
        public static void RestaurarWindow()
        {
            var hwnd = GetActiveWindow();
            ShowWindow(hwnd, SW_RESTAURAR);
        }

        /// <summary>
        /// <para>Mueve la ventana a una posicion dada.</para>
        /// </summary>
        /// <param name="posDelta">Posicion eje x/y.</param>
        /// <param name="nuevaAnchura">Nueva anchura.</param>
        /// <param name="nuevaAltura">Nueva altura.</param>
        public static void MoverWindow(Vector2 posDelta, int nuevaAnchura, int nuevaAltura)
        {
            var hwnd = GetActiveWindow();

            var windowRect = GetWindowRect(hwnd, out WinRect winRect);

            var x = winRect.izquierda + (int)posDelta.x;
            var y = winRect.arriba - (int)posDelta.y;
            MoveWindow(hwnd, x, y, nuevaAnchura, nuevaAltura, false);
        }
        #endregion
    }
}
