using CarRental_Buisness.Helpers;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CarRental.Helper
{
    public static class clsPermissionHelper
    {
        public enum PermissionBehavior { Hide, Disable}

        public static void ApplyPermissions(Form form,PermissionBehavior behavior = PermissionBehavior.Hide)
        {
            if (form == null)
                return;

            ApplyPermissions(form.Controls, behavior);

            if (form.MainMenuStrip != null)
                ApplyPermissions(form.MainMenuStrip.Items, behavior);
        }
        public static void ApplyPermissions(Control rootControl, PermissionBehavior behavior = PermissionBehavior.Hide)
        {
            if (rootControl == null)
                return;

            ApplyPermission(rootControl, behavior);

            if (rootControl.HasChildren)
                ApplyPermissions(rootControl.Controls, behavior);

            if (rootControl is MenuStrip menu)
                ApplyPermissions(menu.Items, behavior);
        }
        private static void ApplyPermissions(Control.ControlCollection controls,PermissionBehavior behavior)
        {
            foreach (Control control in controls)
            {
                ApplyPermission(control, behavior);

                if (control.HasChildren)
                    ApplyPermissions(control.Controls, behavior);

                if (control is MenuStrip menu)
                    ApplyPermissions(menu.Items, behavior);
            }
        }
        private static void ApplyPermissions(ToolStripItemCollection items,PermissionBehavior behavior)
        {
            foreach (ToolStripItem item in items)
            {
                ApplyPermission(item, behavior);

                if (item is ToolStripMenuItem menu)
                    ApplyPermissions(menu.DropDownItems, behavior);
            }
        }
        private static void ApplyPermission(Control control,PermissionBehavior behavior)
        {
            if (!TryResolvePermission(control.Tag, out bool hasPermission))
                return;

            ApplyBehavior(control, hasPermission, behavior);
        }
        private static void ApplyPermission(ToolStripItem item,PermissionBehavior behavior)
        {
            if (!TryResolvePermission(item.Tag, out bool hasPermission))
                return;

            ApplyBehavior(item, hasPermission, behavior);
        }
        private static bool TryResolvePermission(object tag,out bool hasPermission)
        {
            hasPermission = true;

            if (!(tag is string  permissionCode))
                return false;

            permissionCode = permissionCode.Trim();

            if (permissionCode.Length == 0)
                return false;

            hasPermission = clsAuthorizationCache.HasPermission(permissionCode);
            return true;
        }
        private static void ApplyBehavior(Control control,bool hasPermission, PermissionBehavior behavior)
        {
            switch (behavior)
            {
                case PermissionBehavior.Hide:
                    control.Visible = hasPermission;
                    break;

                case PermissionBehavior.Disable:
                    control.Enabled = hasPermission;
                    break;
            }
        }
        private static void ApplyBehavior(ToolStripItem item,bool hasPermission, PermissionBehavior behavior)
        {
            switch (behavior)
            {
                case PermissionBehavior.Hide:
                    item.Visible = hasPermission;
                    break;

                case PermissionBehavior.Disable:
                    item.Enabled = hasPermission;
                    break;
            }
        }
    }
}