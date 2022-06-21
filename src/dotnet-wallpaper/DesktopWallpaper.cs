using System;
using System.Runtime.InteropServices;
using System.Text;
using COLORREF = System.UInt32;
using HRESULT = System.Int32;
using IShellItemArray = System.IntPtr;

namespace ConsoleApp5Wall
{
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public enum DESKTOP_SLIDESHOW_OPTIONS
    {
        DSO_SHUFFLEIMAGES = 0x1
    }

    [Flags]
    public enum DESKTOP_SLIDESHOW_STATE
    {
        DSS_ENABLED = 0x1,
        DSS_SLIDESHOW = 0x2,
        DSS_DISABLED_BY_REMOTE_SESSION = 0x4
    }

    [Flags]
    public enum DESKTOP_SLIDESHOW_DIRECTION
    {
        DSD_FORWARD = 0,
        DSD_BACKWARD = 1
    }

    public enum DESKTOP_WALLPAPER_POSITION
    {
        DWPOS_CENTER = 0,
        DWPOS_TILE = 1,
        DWPOS_STRETCH = 2,
        DWPOS_FIT = 3,
        DWPOS_FILL = 4,
        DWPOS_SPAN = 5
    }

    public static class Const
    {
        public const string IID_IDesktopWallpaper = "B92B56A9-8B55-4E14-9A89-0199BBB6F93B";
        public const string CLSID_DesktopWallpaper = "C2CF3110-460E-4fc1-B9D0-8A1C0C9CC4BD";
    }

    /// <summary>
    /// Provides methods for managing the desktop wallpaper.
    /// </summary>
    [ComImport]
    [Guid(Const.IID_IDesktopWallpaper)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDesktopWallpaper
    {
        /// <summary>
        /// Sets the desktop wallpaper.
        /// </summary>
        /// <param name="monitorID">
        /// The ID of the monitor. This value can be obtained through GetMonitorDevicePathAt. Set this value to NULL to set the wallpaper image on all monitors.
        /// </param>
        /// <param name="wallpaper">
        /// The full path of the wallpaper image file.
        /// </param>
        /// <returns></returns>
        HRESULT SetWallpaper(
            /* [unique][in] */ string monitorID,
            /* [in] */ string wallpaper);

        /// <summary>
        /// Gets the current desktop wallpaper.
        /// </summary>
        /// <param name="monitorID">
        /// The ID of the monitor. This value can be obtained through GetMonitorDevicePathAt.
        ///
        /// This value can be set to NULL. In that case, if a single wallpaper image is displayed on all of the system's monitors, the method returns successfully. If this value is set to NULL and different monitors are displaying different wallpapers or a slideshow is running, the method returns S_FALSE and an empty string in the wallpaper parameter.
        /// </param>
        /// <param name="wallpaper">
        /// The address of a pointer to a buffer that, when this method returns successfully, receives the path to the wallpaper image file. Note that this image could be currently displayed on all of the system's monitors, not just the monitor specified in the monitorID parameter.
        ///
        /// This string will be empty if no wallpaper image is being displayed or if a monitor is displaying a solid color. The string will also be empty if the method fails.
        /// </param>
        /// <returns></returns>
        HRESULT GetWallpaper(
            /* [unique][in] */ string monitorID,
            /* [string][out] */ out StringBuilder wallpaper);

        /// <summary>
        /// Retrieves the unique ID of one of the system's monitors.
        /// </summary>
        /// <param name="monitorIndex">
        /// The number of the monitor. Call GetMonitorDevicePathCount to determine the total number of monitors.
        /// </param>
        /// <param name="monitorID">
        /// A pointer to the address of a buffer that, when this method returns successfully, receives the monitor's ID.
        /// </param>
        /// <returns></returns>
        HRESULT GetMonitorDevicePathAt(
            /* [in] */ uint monitorIndex,
            /* [string][out] */ out StringBuilder monitorID);

        /// <summary>
        /// Retrieves the number of monitors that are associated with the system.
        /// </summary>
        /// <param name="count">
        /// A pointer to a value that, when this method returns successfully, receives the number of monitors.
        /// </param>
        /// <returns></returns>
        HRESULT GetMonitorDevicePathCount(
            /* [out] */ out uint count);

        /// <summary>
        /// Retrieves the display rectangle of the specified monitor.
        /// </summary>
        /// <param name="monitorID">
        /// The ID of the monitor to query. You can get this value through GetMonitorDevicePathAt.
        /// </param>
        /// <param name="displayRect">
        /// A pointer to a RECT structure that, when this method returns successfully, receives the display rectangle of the monitor specified by monitorID, in screen coordinates.
        /// </param>
        /// <returns></returns>
        HRESULT GetMonitorRECT(
            /* [in] */ string monitorID,
            /* [out] */ out RECT displayRect);

        /// <summary>
        /// Sets the color that is visible on the desktop when no image is displayed or when the desktop background has been disabled. This color is also used as a border when the desktop wallpaper does not fill the entire screen.
        /// </summary>
        /// <param name="color">
        /// A COLORREF value that specifies the background RGB color value.
        /// </param>
        /// <returns></returns>
        HRESULT SetBackgroundColor(
            /* [in] */ COLORREF color);

        /// <summary>
        /// Retrieves the color that is visible on the desktop when no image is displayed or when the desktop background has been disabled. This color is also used as a border when the desktop wallpaper does not fill the entire screen.
        /// </summary>
        /// <param name="color">
        /// A pointer to a COLORREF value that, when this method returns successfully, receives the RGB color value. If this method fails, this value is set to 0.
        /// </param>
        /// <returns></returns>
        HRESULT GetBackgroundColor(
            /* [out] */ out COLORREF color);

        /// <summary>
        /// Sets the display option for the desktop wallpaper image, determining whether the image should be centered, tiled, or stretched.
        /// </summary>
        /// <param name="position">
        /// One of the DESKTOP_WALLPAPER_POSITION enumeration values that specify how the image will be displayed on the system's monitors.
        /// </param>
        /// <returns></returns>
        HRESULT SetPosition(
            /* [in] */ DESKTOP_WALLPAPER_POSITION position);

        /// <summary>
        /// Retrieves the current display value for the desktop background image.
        /// </summary>
        /// <param name="position">
        /// A pointer to a value that, when this method returns successfully, receives one of the DESKTOP_WALLPAPER_POSITION enumeration values that specify how the image is being displayed on the system's monitors.
        /// </param>
        /// <returns></returns>
        HRESULT GetPosition(
            /* [out] */ out DESKTOP_WALLPAPER_POSITION position);

        /// <summary>
        /// Specifies the images to use for the desktop wallpaper slideshow.
        /// </summary>
        /// <param name="items">
        /// A pointer to an IShellItemArray that contains the slideshow images. This array can contain individual items stored in the same container (files stored in a folder), or it can contain a single item which is the container itself (a folder that contains images). Any other configuration of the array will cause this method to fail.
        /// </param>
        /// <returns></returns>
        HRESULT SetSlideshow(
            /* [in] */  IShellItemArray items);

        HRESULT GetSlideshow(
            /* [out] */ out IShellItemArray items);

        /// <summary>
        /// Sets the desktop wallpaper slideshow settings for shuffle and timing.
        /// </summary>
        /// <param name="options">
        /// Set to either 0 to disable shuffle or the following value.
        ///
        /// DSO_SHUFFLEIMAGES (0x01)
        /// Enable shuffle; advance through the slideshow in a random order.
        /// </param>
        /// <param name="slideshowTick">
        /// The amount of time, in milliseconds, between image transitions.
        /// </param>
        /// <returns></returns>
        HRESULT SetSlideshowOptions(
            /* [in] */ DESKTOP_SLIDESHOW_OPTIONS options,
            /* [in] */ uint slideshowTick);

        /// <summary>
        /// Gets the current desktop wallpaper slideshow settings for shuffle and timing.
        /// </summary>
        /// <param name="options">
        /// A pointer to a value that, when this method returns successfully, receives either 0 to indicate that shuffle is disabled or the following value.
        ///
        /// DSO_SHUFFLEIMAGES (0x01)
        /// Shuffle is enabled; the images are shown in a random order.
        /// </param>
        /// <param name="slideshowTick">
        /// A pointer to a value that, when this method returns successfully, receives the interval between image transitions, in milliseconds.
        /// </param>
        /// <returns></returns>
        HRESULT GetSlideshowOptions(
            /* [out] */ out DESKTOP_SLIDESHOW_OPTIONS options,
            /* [out] */ out uint slideshowTick);

        /// <summary>
        /// Switches the wallpaper on a specified monitor to the next image in the slideshow.
        /// </summary>
        /// <param name="monitorID">
        /// The ID of the monitor on which to change the wallpaper image. This ID can be obtained through the GetMonitorDevicePathAt method. If this parameter is set to NULL, the monitor scheduled to change next is used.
        /// </param>
        /// <param name="direction">
        /// The direction that the slideshow should advance. One of the following DESKTOP_SLIDESHOW_DIRECTION values:
        ///
        /// DSD_FORWARD (0)
        /// Advance the slideshow forward.
        ///
        /// DSD_BACKWARD (1)
        /// Advance the slideshow backward.
        /// </param>
        /// <returns></returns>
        HRESULT AdvanceSlideshow(
            /* [unique][in] */ string monitorID,
            /* [in] */ DESKTOP_SLIDESHOW_DIRECTION direction);

        /// <summary>
        /// Gets the current status of the slideshow.
        /// </summary>
        /// <param name="state">
        /// A pointer to a DESKTOP_SLIDESHOW_STATE value that, when this method returns successfully, receives one or more of the following flags.
        ///
        /// DSS_ENABLED (0x01)
        /// Slideshows are enabled.
        ///
        /// DSS_SLIDESHOW (0x02)
        /// A slideshow is currently configured.
        ///
        /// DSS_DISABLED_BY_REMOTE_SESSION (0x04)
        /// A remote session has temporarily disabled the slideshow.
        /// </param>
        /// <returns></returns>
        HRESULT GetStatus(
            /* [out] */ out DESKTOP_SLIDESHOW_STATE state);

        /// <summary>
        /// Enables or disables the desktop background.
        /// </summary>
        /// <param name="enable">
        /// TRUE to enable the desktop background, FALSE to disable it.
        /// </param>
        /// <returns></returns>
        HRESULT Enable(
            /* [in] */ bool enable);
    }

    [ComImport]
    [Guid(Const.CLSID_DesktopWallpaper)]
    public class DesktopWallpaper
    {
    }
}
