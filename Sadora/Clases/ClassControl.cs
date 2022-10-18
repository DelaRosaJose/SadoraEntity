using DevExpress.Xpf.Grid;
using Sadora.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sadora.Clases
{
    class ClassControl
    {
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder sb = new StringBuilder();
            byte[] stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static string setPropBinding(string Consulta, TextBox Enviador, bool OnlySearch = false) //Este metodo se encarga de validar cualquier campo de la ventana que este llamando otro mantenimiento, Ejemplo(Campo clase de clientes contiene(ClaseID, Detalle de la clase que es el nombre)), 
        {                                                                       // el metodo recibe el textbox que le envia la info, la consulta que debe buscar con esa info y el textbox donde debe depositar el resultado.
            if (!OnlySearch)
            {
                if (Enviador.Text == null || Enviador.Text == "")
                    Consulta += "0";

                Consulta += Enviador.Text;
            }

            SqlDataReader reader = Clases.ClassData.runSqlDataReader(Consulta, null, "CommandText");
            //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

            if (reader.HasRows) //Validamos si el datareader trajo data.
            {
                if (reader.Read()) //Si puede leer la informacion
                {
                    var result = reader["Nombre"].ToString(); //Asignamos el resultado de la columna "Nombre" del datareader en el textbox que le indicamos en el parametro previamente identificado.
                    reader.Close(); //Cerramos el datareader
                    reader.Dispose(); //Cortamos la conexion del datareader
                    return result;
                }
                reader.Close(); //Cerramos el datareader
                reader.Dispose(); //Cortamos la conexion del datareader
                return default;
            }
            else //Si no trajo data
            {
                //Recibidor.Clear(); //Dejamos en blanco el campo que recibe
                reader.Close(); //limpiamos el reader
                reader.Dispose();
                return default;
            }

        }

        public static void setValidador(string Consulta, TextBox Enviador, TextBox Recibidor, bool OnlySearch = false) //Este metodo se encarga de validar cualquier campo de la ventana que este llamando otro mantenimiento, Ejemplo(Campo clase de clientes contiene(ClaseID, Detalle de la clase que es el nombre)), 
        {                                                                       // el metodo recibe el textbox que le envia la info, la consulta que debe buscar con esa info y el textbox donde debe depositar el resultado.
            if (!OnlySearch)
            {
                if (Enviador.Text == null || Enviador.Text == "")
                    Consulta += "0";

                Consulta += Enviador.Text;
            }

            SqlDataReader reader = Clases.ClassData.runSqlDataReader(Consulta, null, "CommandText");
            //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

            if (reader != null && reader.HasRows) //Validamos si el datareader trajo data.
            {
                if (reader.Read()) //Si puede leer la informacion
                {
                    Recibidor.Text = reader["Nombre"].ToString(); //Asignamos el resultado de la columna "Nombre" del datareader en el textbox que le indicamos en el parametro previamente identificado.
                    reader.NextResult();
                }
                reader.Close(); //Cerramos el datareader
                reader.Dispose(); //Cortamos la conexion del datareader
            }
            else  //Si no trajo data
            {
                Recibidor.Clear(); //Dejamos en blanco el campo que recibe
                if (reader != null)
                {
                    reader.Close(); //limpiamos el reader
                    reader.Dispose();

                }
            }
        }

        public static void ClearControl(List<Control> listaControles)
        {
            if (listaControles != null)
            {
                foreach (Control control in listaControles)
                {
                    if (control is TextBox)
                        (control as TextBox).Clear();

                    else if (control is PasswordBox)
                        (control as PasswordBox).Clear();

                    else if (control is DatePicker)
                        (control as DatePicker).Text = "";
                }
            }
            listaControles.Clear();
        }

        public static void ActivadorControlesReadonly(List<Control> listaControl, bool funcion, bool editando, bool Agregando = false, List<Control> listaControles = null, List<Control> listaControlesSoloLimpiar = null)
        {
            if (listaControl != null)
            {
                foreach (Control control in listaControl)
                {
                    if (editando == true)
                    {
                        control.IsEnabled = funcion;

                        if (control is TextBox)
                            (control as TextBox).IsReadOnly = !funcion;

                        else if (control is PasswordBox)
                            (control as PasswordBox).IsEnabled = funcion;

                        else if (control is ComboBox)
                            (control as ComboBox).IsEnabled = funcion;

                        else if (control is CheckBox)
                            (control as CheckBox).IsEnabled = funcion;

                        else if (control is DatePicker)
                            (control as DatePicker).IsEnabled = funcion;
                    }
                    else if (Agregando == true)
                    {
                        control.IsEnabled = !funcion;

                        if (control is TextBox)
                            (control as TextBox).IsReadOnly = funcion;

                        else if (control is PasswordBox)
                            (control as PasswordBox).IsEnabled = !funcion;

                        else if (control is ComboBox)
                            (control as ComboBox).IsEnabled = !funcion;

                        else if (control is CheckBox)
                            (control as CheckBox).IsEnabled = !funcion;

                        else if (control is DatePicker)
                            (control as DatePicker).IsEnabled = !funcion;
                    }
                    else
                    {
                        control.IsEnabled = funcion;

                        if (control is TextBox)
                        {
                            (control as TextBox).IsReadOnly = funcion;
                        }
                        else if (control is PasswordBox)
                        {
                            (control as PasswordBox).IsEnabled = !funcion;
                        }
                        else if (control is ComboBox)
                        {
                            (control as ComboBox).IsEnabled = !funcion;
                        }
                        else if (control is CheckBox)
                        {
                            (control as CheckBox).IsEnabled = !funcion;
                        }
                        else if (control is DatePicker)
                        {
                            (control as DatePicker).IsEnabled = !funcion;
                        }
                    }
                    if (funcion == false)
                    {
                        if (control is TextBox)
                        {
                            (control as TextBox).Clear();
                        }
                        else if (control is PasswordBox)
                        {
                            (control as PasswordBox).Clear();  //ClearValue();
                        }
                        else if (control is DatePicker)
                        {
                            (control as DatePicker).Text = "";  //ClearValue();
                        }
                    }
                    if (listaControles != null)
                    {
                        control.IsEnabled = !funcion;
                    }
                }
                listaControl.Clear();
            }
            if (listaControles != null)
            {
                foreach (Control control in listaControles)
                {
                    control.IsEnabled = funcion;

                    if (control is TextBox)
                    {
                        (control as TextBox).Clear();
                    }
                    else if (control is PasswordBox)
                    {
                        (control as PasswordBox).Clear();  //ClearValue();
                    }
                    else if (control is DatePicker)
                    {
                        (control as DatePicker).Text = "";  //ClearValue();
                    }
                }
                listaControles.Clear();
            }
            if (listaControlesSoloLimpiar != null)
            {
                foreach (Control control in listaControlesSoloLimpiar)
                {
                    if (control is TextBox)
                        (control as TextBox).Clear();

                    else if (control is PasswordBox)
                        (control as PasswordBox).Clear();  //ClearValue();

                    else if (control is DatePicker)
                        (control as DatePicker).Text = "";  //ClearValue();
                }
                listaControlesSoloLimpiar.Clear();
            }
        }

        public static string ValidadorControles(List<Control> listaControl)
        {
            string Linea = "Debe Completar los Campos: ";
            int contador = 0;

            foreach (Control control in listaControl)
            {
                if (control is TextBox)
                {
                    if ((control as TextBox).Text == "")
                    {
                        if (contador == 0)
                            control.Focus();

                        contador++;

                        if (Linea == "Debe Completar los Campos: ")
                            Linea += control.Name;
                        else
                            Linea = Linea + ", " + control.Name;

                        Linea = Linea.Replace("txt", "").Replace("tbx", "");
                    }
                }
                else if (control is DatePicker)
                {
                    if ((control as DatePicker).Text == "")
                    {
                        if (contador == 0)
                            control.Focus();

                        contador++;

                        if (Linea == "Debe Completar los Campos: ")
                            Linea += control.Name;

                        else
                            Linea = Linea + ", " + control.Name;

                        Linea = Linea.Replace("txt", "").Replace("tbx", "").Replace("dtp", "");
                    }
                }
            }
            return Linea;
        }


        public static bool ValidateIdentityCard(string identityCard)
        {
            string replaceScore = identityCard.Replace("-", "");

            if (Regex.IsMatch(replaceScore, "^[0-9]{11}$"))
            {
                string identityCardSub = replaceScore.Substring(0, replaceScore.Length - 1);
                string checkerNumber = replaceScore.Substring(replaceScore.Length - 1, 1);
                int sum = 0;
                for (int i = 0; i < identityCardSub.Length; i++)
                {
                    int module;
                    if ((i % 2) == 0)
                        module = 1;
                    else
                        module = 2;

                    int result = int.Parse(identityCardSub.Substring(i, 1)) * module;
                    if (result > 9)
                    {
                        string resultString = result.ToString();
                        string one = resultString.Substring(0, 1);
                        string two = resultString.Substring(1, 1);
                        result = int.Parse(one) + int.Parse(two);

                    }

                    sum += result;
                }

                int numberValidate = (10 - (sum % 10)) % 10;

                bool identityCardValid;
                if ((numberValidate == int.Parse(checkerNumber)) && (identityCardSub.Substring(0, 3) != "000"))
                    identityCardValid = true;
                else
                    identityCardValid = false;

                return identityCardValid;

            }
            else
                return false;
        }

        public static SqlDataReader getDatosCedula(string Cedula)
        {
            SqlDataReader tabla = null;
            if (Cedula.Length > 13)
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost("No puede ingresar mas de 13 digitos en el RNC");
                frm.ShowDialog();
            }
            else
            {
                SqlDataReader Result = ClassData.runSqlDataReader("select count(*) as Cant from TcliClientes where RNC = '" + Cedula + "' ", null, "CommandText");
                if (Result.HasRows && Result.Read() && Convert.ToInt32(Result["Cant"].ToString()) > 0)
                {
                    ClassVariables.ExistClient = true;
                    Result.Close();
                    Result.Dispose();
                }
                else
                {
                    Result.Close();
                    Result.Dispose();
                    tabla = ClassData.runSqlDataReader("exec spCedula '" + Cedula + "'", null, "CommandText");
                }
            }
            return tabla;
        }

        public static void CampoSoloPermiteNumeros(KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || new Key[] { Key.Decimal, Key.Enter, Key.Tab, Key.Delete, Key.Back, Key.Left, Key.Right }.Contains(e.Key))
                e.Handled = false;
            else
                e.Handled = true;
        }
        public static void CampoSoloPermiteDecimales(KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || new Key[] { Key.OemPeriod, Key.Decimal, Key.Enter, Key.Tab, Key.Delete, Key.Back, Key.Left, Key.Right }.Contains(e.Key))
                e.Handled = false;
            else
                e.Handled = true;
        }

        public static void SetGridReadOnly(GridControl Grid, List<String> ListaColumnas = null, Boolean AllowEdit = true)
        {
            if (ListaColumnas == null)
            {
                foreach (GridColumn col in Grid.Columns)
                    col.ReadOnly = AllowEdit;
            }
            else
            {
                foreach (String Valor in ListaColumnas)
                    Grid.Columns[Valor].ReadOnly = AllowEdit;
            }
        }

        public static void UpdateNameUser()
        {
            SqlDataReader reader = Clases.ClassData.runSqlDataReader("select * from TsysUsuarios where UsuarioID = " + ClassVariables.UsuarioID, null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

            try
            {
                if (reader.HasRows) //Validamos si el datareader trajo data.
                {
                    if (reader.Read()) //Si puede leer la informacion
                    {
                        ClassVariables.ClasesVariables.UserName = reader["Nombre"].ToString();
                        reader.NextResult();
                    }
                    reader.Close(); //Cerramos el datareader
                    reader.Dispose(); //Cortamos la conexion del datareader
                }
                else //Si no trajo data
                {
                    reader.Close(); //limpiamos el reader
                    reader.Dispose();
                }
            }
            catch { }
        }

        //public static void GridAllowEdit(DevExpress.Xpf.Grid.GridControl Grid, List<String> ListaColumnas, Boolean AllowEdit, string opcion = "--AllowEdit-- o --Visible--") 
        //{
        //    DevExpress.Utils.DefaultBoolean DevExbool;

        //    if (AllowEdit)
        //    {
        //        DevExbool = DevExpress.Utils.DefaultBoolean.True;
        //    }
        //    else
        //    {
        //        DevExbool = DevExpress.Utils.DefaultBoolean.False;
        //    }

        //    foreach (String Valor in ListaColumnas)
        //    {
        //        Grid.Columns[Valor].AllowEditing = DevExbool;

        //        //Grid.Columns[Valor].Visible    = false;
        //    }
        //}

        public static void GridCheckEdit(DevExpress.Xpf.Grid.GridControl Grid, string Columna, Boolean Opcion)
        {
            Grid.View.MoveFirstRow();
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                Grid.SetFocusedRowCellValue(Columna, Opcion);
                Grid.View.MoveNextRow();
            }
        }

        #region LimpiadorGeneral(UIElementCollection view) --**-- Metodo que se encarga de limpiar todos los controles de una ventana.
        public static void LimpiadorGeneral(UIElementCollection view)
        {
            try
            {
                for (int i = 0; i < view.Count; i++)
                {
                    var Element = view[i];

                    if (Element is Grid Grid)
                        LimpiadorGeneral(Grid.Children);

                    else if (Element is ScrollViewer && (Element as ScrollViewer).Content is Grid)
                        LimpiadorGeneral(((Element as ScrollViewer).Content as Grid).Children);

                    else if (Element is StackPanel StackPanel)
                        LimpiadorGeneral(StackPanel.Children);

                    else if (Element is Border Border)
                        LimpiadorGeneral((Border.Child as StackPanel).Children);

                    else if (Element is CustomElements.UscTextboxGeneral UscTextboxGeneral)
                        UscTextboxGeneral.Text = default;

                    else if (Element is CustomElements.UscTextboxTelefono UscTextboxTelefono)
                        UscTextboxTelefono.Text = default;

                    else if (Element is CustomElements.UscTextboxNumerico UscTextboxNumerico)
                        UscTextboxNumerico.Number = 0.ToString();

                    else if (Element is CustomElements.UscTextboxButtonGeneral UscTextboxButtonGeneral)
                        UscTextboxButtonGeneral.Text = default;

                    else if (Element is CustomElements.UscCheckBoxGeneral UscCheckBoxGeneral)
                        UscCheckBoxGeneral.IsChecked = default;

                    else if ((Element is CustomElements.UscBotonesGenerales) || Element is MaterialDesignThemes.Wpf.Snackbar || Element is MaterialDesignThemes.Wpf.PopupBox || Element is MaterialDesignThemes.Wpf.DialogHost)
                        continue;

                    else if (Element is CustomElements.UscDatePickerGeneral UscDatePickerGeneral)
                        UscDatePickerGeneral.Date = DateTime.Now;

                    else if (Element is CustomElements.UscComboBoxGeneral UscComboBoxGeneral)
                        UscComboBoxGeneral.Text = default;

                    else
                        new Administracion.FrmCompletarCamposHost($"Advertencia: \nEste control no esta registrado {Element} --ClassControl/LimpiadorGeneral \nComuniquese con soporte").ShowDialog();

                }
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }
        #endregion

        #region CanSaveView(UIElementCollection view) --**-- Metodo que se encarga de validar todos los controles de una ventana y retorna si se puede guardar.
        public static bool CanSaveView(UIElementCollection view)
        {
            double opacity = 0.30;
            bool CanSave = true;
            try
            {
                for (int i = 0; i < view.Count; i++)
                {

                    var Element = view[i];

                    if (Element is Grid Grid)
                        CanSave &= CanSaveView(Grid.Children);

                    else if (Element is ScrollViewer && (Element as ScrollViewer).Content is Grid)
                        CanSave &= CanSaveView(((Element as ScrollViewer).Content as Grid).Children);

                    else if (Element is StackPanel StackPanel)
                        CanSave &= CanSaveView(StackPanel.Children);

                    else if (Element is Border Border)
                        CanSave &= CanSaveView((Border.Child as StackPanel).Children);

                    else if ((Element is CustomElements.UscBotonesGenerales) || Element is MaterialDesignThemes.Wpf.Snackbar || Element is MaterialDesignThemes.Wpf.PopupBox || Element is MaterialDesignThemes.Wpf.DialogHost)
                        continue;

                    else if (Element is CustomElements.UscTextboxGeneral UscTextBoxGeneral)
                    {
                        bool and = !new string[] { string.Empty, default }.Contains(UscTextBoxGeneral.Text) || UscTextBoxGeneral.GuardarCampoVacio;
                        CanSave &= and;

                        if (and)
                            UscTextBoxGeneral.ColorCampoVacio = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"));
                        else
                            UscTextBoxGeneral.ColorCampoVacio = new SolidColorBrush(Colors.DarkRed) { Opacity = opacity };
                    }

                    else if (Element is CustomElements.UscTextboxTelefono UscTextboxTelefono)
                    {
                        bool and = !new string[] { string.Empty, default }.Contains(UscTextboxTelefono.Text) || UscTextboxTelefono.GuardarCampoVacio;
                        CanSave &= and;

                        if (and)
                            UscTextboxTelefono.ColorCampoVacio = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"));
                        else
                            UscTextboxTelefono.ColorCampoVacio = new SolidColorBrush(Colors.DarkRed) { Opacity = opacity };
                    }

                    else if (Element is CustomElements.UscTextboxNumerico UscTextboxNumerico)
                    {
                        bool and = !new string[] { string.Empty, default }.Contains(UscTextboxNumerico.Number) || UscTextboxNumerico.GuardarCampoVacio;
                        CanSave &= and;

                        if (and)
                            UscTextboxNumerico.ColorCampoVacio = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"));
                        else
                            UscTextboxNumerico.ColorCampoVacio = new SolidColorBrush(Colors.DarkRed) { Opacity = opacity };
                    }

                    else if (Element is CustomElements.UscTextboxButtonGeneral UscTextboxButtonGeneral)
                    {
                        bool and = (!new string[] { string.Empty, default }.Contains(UscTextboxButtonGeneral.Text) && !new string[] { string.Empty, default }.Contains(UscTextboxButtonGeneral.ResultText.Text)) || UscTextboxButtonGeneral.GuardarCampoVacio;
                        CanSave &= and;

                        if (and)
                            UscTextboxButtonGeneral.ColorCampoVacio = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"));
                        else
                            UscTextboxButtonGeneral.ColorCampoVacio = new SolidColorBrush(Colors.DarkRed) { Opacity = opacity };
                    }

                    else if (Element is CustomElements.UscCheckBoxGeneral UscCheckBoxGeneral)
                    {
                        bool and = UscCheckBoxGeneral.IsChecked != default;
                        CanSave &= and;

                        if (and)
                            UscCheckBoxGeneral.ColorCampoVacio = default;
                        else
                            UscCheckBoxGeneral.ColorCampoVacio = new SolidColorBrush(Colors.DarkRed) { Opacity = opacity };
                    }

                    else if (Element is CustomElements.UscDatePickerGeneral UscDatePickerGeneral)
                    {
                        bool and = UscDatePickerGeneral.Date != default;
                        CanSave &= and;

                        if (and)
                            UscDatePickerGeneral.Background = default;
                        else
                            UscDatePickerGeneral.Background = new SolidColorBrush(Colors.DarkRed) { Opacity = opacity };
                    }

                    else if (Element is CustomElements.UscComboBoxGeneral UscComboBoxGeneral)
                    {
                        bool and = !new string[] { string.Empty, default }.Contains(UscComboBoxGeneral.Text);
                        CanSave &= and;

                        if (and)
                            UscComboBoxGeneral.ColorCampoVacio = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"));
                        else
                            UscComboBoxGeneral.ColorCampoVacio = new SolidColorBrush(Colors.DarkRed) { Opacity = opacity };
                    }

                    else
                        new Administracion.FrmCompletarCamposHost($"Advertencia: \nEste control no esta registrado {Element} --ClassControl/CanSaveView \nComuniquese con soporte").ShowDialog();


                }

                return CanSave;
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
                return false;
            }
        }
        #endregion

        #region SetDefaultColorELement(UIElementCollection view) --**-- Metodo que se encarga de validar todos los controles de una ventana y retorna si se puede guardar.
        public static void SetDefaultColorELement(UIElementCollection view)
        {
            string Color = "#4CEDEDED";

            try
            {
                for (int i = 0; i < view.Count; i++)
                {
                    var Element = view[i];

                    if (Element is Grid Grid)
                        SetDefaultColorELement(Grid.Children);

                    else if (Element is ScrollViewer && (Element as ScrollViewer).Content is Grid)
                        SetDefaultColorELement(((Element as ScrollViewer).Content as Grid).Children);

                    else if (Element is StackPanel StackPanel)
                        SetDefaultColorELement(StackPanel.Children);

                    else if (Element is Border Border)
                        SetDefaultColorELement((Border.Child as StackPanel).Children);

                    else if ((Element is CustomElements.UscBotonesGenerales) || Element is MaterialDesignThemes.Wpf.Snackbar || Element is MaterialDesignThemes.Wpf.PopupBox || Element is MaterialDesignThemes.Wpf.DialogHost)
                        continue;

                    else if (Element is CustomElements.UscTextboxGeneral UscTextBoxGeneral)
                        UscTextBoxGeneral.ColorCampoVacio = (SolidColorBrush)new BrushConverter().ConvertFrom(Color);

                    else if (Element is CustomElements.UscTextboxTelefono UscTextboxTelefono)
                        UscTextboxTelefono.ColorCampoVacio = (SolidColorBrush)new BrushConverter().ConvertFrom(Color);

                    else if (Element is CustomElements.UscTextboxNumerico UscTextboxNumerico)
                        UscTextboxNumerico.ColorCampoVacio = (SolidColorBrush)new BrushConverter().ConvertFrom(Color);

                    else if (Element is CustomElements.UscTextboxButtonGeneral UscTextboxButtonGeneral)
                        UscTextboxButtonGeneral.ColorCampoVacio = (SolidColorBrush)new BrushConverter().ConvertFrom(Color);

                    else if (Element is CustomElements.UscCheckBoxGeneral UscCheckBoxGeneral)
                        UscCheckBoxGeneral.ColorCampoVacio = default;

                    else if (Element is CustomElements.UscDatePickerGeneral UscDatePickerGeneral)
                        UscDatePickerGeneral.Background = default;

                    else if (Element is CustomElements.UscComboBoxGeneral UscComboBoxGeneral)
                        UscComboBoxGeneral.ColorCampoVacio = (SolidColorBrush)new BrushConverter().ConvertFrom(Color);

                    else
                        new Administracion.FrmCompletarCamposHost($"Advertencia: \nEste control no esta registrado {Element} --ClassControl/SetDefaultColorELement \nComuniquese con soporte").ShowDialog();
                }
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }
        #endregion

        #region PuedeEscribirEnCampo(string Estatus, KeyEventArgs e) --**-- 
        public static void PuedeEscribirEnCampo(string Estatus, KeyEventArgs e)
        {
            try
            {
                if (Estatus == "Modo Consulta")
                {
                    e.Handled = true;
                    return;
                }

                if (e.Key == Key.Enter)
                    InputManager.Current.ProcessInput(new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) { RoutedEvent = Keyboard.KeyDownEvent });

            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }

        #endregion

        public static void PasarConEnterProximoCampo(KeyEventArgs e, string EstadoMainWindows, bool EnterPasarProximoCampo)
        {
            try
            {
                if (e.Key == Key.Enter && EstadoMainWindows != "Modo Consulta" && EnterPasarProximoCampo)
                    InputManager.Current.ProcessInput(new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) { RoutedEvent = Keyboard.KeyDownEvent });
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }

        public static void PresentadorSnackBar(MaterialDesignThemes.Wpf.Snackbar SnackbarThree, string Message)
        {
            if (SnackbarThree.MessageQueue is { } messageQueue)
                Task.Factory.StartNew(() => messageQueue.Enqueue(Message));
        }

        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
                dataTable.Columns.Add(prop.Name);

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                    values[i] = Props[i].GetValue(item, null);

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        public static bool IsValidDrCedula(string drCedula)
        {
            // Valid format?
            if (drCedula == null)
                return false;
            else
            {
                drCedula = Regex.Replace(drCedula, "[^0-9]", string.Empty); // Only keep #s.
                if (drCedula.Equals(null) || !drCedula.Length.Equals(11) || long.Parse(drCedula).Equals(0))
                    return false;
            }

            // Validate.
            int sum = 0;
            for (int i = 0; i < 10; ++i)
            {
                int n = ((i + 1) % 2 != 0 ? 1 : 2) * int.Parse(drCedula.Substring(i, 1));
                sum += (n <= 9 ? n : n % 10 + 1);
            }
            int dig = ((10 - sum % 10) % 10);
            return (dig.Equals(int.Parse(drCedula.Substring(10, 1))) ? true : false);
        }

        public static bool IsValidDrRnc(string drRnc)
        {
            // Valid format?
            if (drRnc == null)
                return false;
            else
            {
                drRnc = Regex.Replace(drRnc, "[^0-9]", string.Empty); // Only keep #s.
                if (!drRnc.Length.Equals(9) || long.Parse(drRnc).Equals(0))
                    return false;
            }

            // Validate.
            int sum = 0;
            for (int i = 0; i < 8; ++i)
                sum += int.Parse(drRnc.Substring(i, 1)) * int.Parse("79865432".Substring(i, 1));

            int div = sum / 11;
            int res = sum - (div * 11);
            int dig = (res > 1 ? 11 - res : (res.Equals(0) ? 2 : 1));
            return (dig.Equals(int.Parse(drRnc.Substring(8, 1))) ? true : false);
        }

        public static bool IsValidCedulaORNC(string CedulaORNC, string EstadoVentana) => (IsValidDrCedula(CedulaORNC) || IsValidDrRnc(CedulaORNC)) && !new string[] { "Modo Consulta", "Modo Busqueda" }.Contains(EstadoVentana);

        public static DGII_RNC BuscarPorRNCoCedula(string Cedula)
        {
            using SadoraEntity db = new SadoraEntity();

            CeduladosJCE JCE = db.CeduladosJCEs.Where(x => x.Cedula == Cedula).FirstOrDefault();
            if (JCE != default)
                return new DGII_RNC() { RazonSocial = $"{JCE.Nombres} {JCE.Apellido1}", NombreComercial = JCE.Nombres };
            else
                return db.DGII_RNC.Where(x => x.RNC == Cedula && x.Estado == "ACTIVO").FirstOrDefault();
        }

    }
}
