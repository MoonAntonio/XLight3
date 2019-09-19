//                                  ┌∩┐(◣_◢)┌∩┐
//                                                                              \\
// WindowsController.cs (19/09/2019)                                            \\
// Autor: Antonio Mateo (.\Moon Antonio)    antoniomt.moon@gmail.com            \\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.EventSystems;
#endregion

namespace XLight.Windows
{
    /// <summary>
    /// <para>Controlador de la ventana de la aplicacion.</para>
    /// </summary>
    [AddComponentMenu("XLight/Controlador/Windows Controller", 100)]
    public class WindowsController : MonoBehaviour, IDragHandler
    {
        #region Variables Publicas
        /// <summary>
        /// <para>Tamaño de la ventana.</para>
        /// </summary>
        public Vector2Int windowSize;
        /// <summary>
        /// <para>Tamaño de bordes de la ventana.</para>
        /// </summary>
        public Vector2Int bordesSize;
        #endregion

        #region Variables Privadas
        /// <summary>
        /// <para>Contiene el valor delta de la ventana. Al ser arrastrada.</para>
        /// </summary>
        private Vector2 valorDelta = Vector2.zero;
        /// <summary>
        /// <para>Determina si la ventana esta maximizada.</para>
        /// </summary>
        private bool isMaximizada;
        #endregion

        #region API
        /// <summary>
        /// <para>Activa los bordes.</para>
        /// </summary>
        public void OnBordes()
        {
            if (WindowsExtension.isMarco) return;

            WindowsExtension.SetWindowConMarco();
            WindowsExtension.MoverWindow(Vector2Int.zero, Screen.width + bordesSize.x, Screen.height + bordesSize.y); // Compensacion del desplazamiento del borde.
        }

        /// <summary>
        /// <para>Desactiva los bordes.</para>
        /// </summary>
        public void OnNoBordes()
        {
            if (!WindowsExtension.isMarco)  return;

            WindowsExtension.SetWindowSinMarco();
            WindowsExtension.MoverWindow(Vector2Int.zero, Screen.width - bordesSize.x, Screen.height - bordesSize.y);
        }

        /// <summary>
        /// <para>Resetea el tamaño de la ventana.</para>
        /// </summary>
        public void ResetWindowSize()
        {
            WindowsExtension.MoverWindow(Vector2Int.zero, windowSize.x, windowSize.y);
        }

        /// <summary>
        /// <para>Cierra la APP.</para>
        /// </summary>
        public void OnCerrar()
        {
            EventSystem.current.SetSelectedGameObject(null);
            Application.Quit();
        }

        /// <summary>
        /// <para>Minimiza la APP.</para>
        /// </summary>
        public void OnMinimizar()
        {
            EventSystem.current.SetSelectedGameObject(null);
            WindowsExtension.MinimizarWindow();
        }

        /// <summary>
        /// <para>Maximiza la APP.</para>
        /// </summary>
        public void OnMaximizar()
        {
            EventSystem.current.SetSelectedGameObject(null);

            if (isMaximizada)
                WindowsExtension.RestaurarWindow();
            else
                WindowsExtension.MaximizarWindow();

            isMaximizada = !isMaximizada;
        }

        /// <summary>
        /// <para>Al arrastrar la ventana.</para>
        /// </summary>
        /// <param name="data"></param>
        public void OnDrag(PointerEventData data)
        {
            if (WindowsExtension.isMarco) return;

            valorDelta += data.delta;
            if (data.dragging)
            {
                WindowsExtension.MoverWindow(valorDelta, Screen.width, Screen.height);
            }
        }
        #endregion
    }
}
