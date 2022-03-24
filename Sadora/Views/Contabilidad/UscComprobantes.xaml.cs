using Sadora.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sadora.Contabilidad
{
    /// <summary>
    /// Lógica de interacción para UscComprobantes.xaml
    /// </summary>
    public partial class UscComprobantes : UserControl
    {
        public UscComprobantes()
        {
            InitializeComponent();
            Name = "UscComprobantes";
        }

        bool Imprime;
        bool Agrega;
        bool Modifica;

        bool Inicializador = false;
        DataTable tabla;
        SqlDataReader reader;
        string Estado;
        string Lista;
        int TransaccionID;
        int LastTransaccionID;
        string last;

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            Inicializador = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Inicializador == true)
            {
                Imprime = ClassVariables.Imprime;
                Agrega = ClassVariables.Agrega;
                Modifica = ClassVariables.Modifica;

                this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                Inicializador = false;
            }
        }

        private void BtnPrimerRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               txtNombre
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            setDatos(0, "1");
            BtnPrimerRegistro.IsEnabled = false;
            BtnAnteriorRegistro.IsEnabled = false;
        }

        private void BtnAnteriorRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               txtNombre
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            try
            {
                TransaccionID = Convert.ToInt32(txtComprobanteID.Text) - 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }


            if (TransaccionID <= 1)
            {
                BtnPrimerRegistro.IsEnabled = false;
                BtnAnteriorRegistro.IsEnabled = false;
                setDatos(0, "1");
            }
            else
            {
                setDatos(0, TransaccionID.ToString());
            }
        }

        private void BtnProximoRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               txtNombre
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            try
            {
                TransaccionID = Convert.ToInt32(txtComprobanteID.Text) + 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }

            if (TransaccionID >= LastTransaccionID)
            {
                BtnUltimoRegistro.IsEnabled = false;
                BtnProximoRegistro.IsEnabled = false;
                setDatos(0, LastTransaccionID.ToString());
            }
            else
            {
                setDatos(0, TransaccionID.ToString());
            }
        }

        private void BtnUltimoRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               txtNombre
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            setDatos(-1, "1");
            BtnUltimoRegistro.IsEnabled = false;
            BtnProximoRegistro.IsEnabled = false;
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {

            if (Estado == "Modo Consulta")
            {
                last = txtComprobanteID.Text;
                SetEnabledButton("Modo Busqueda");
            }
            else if (Estado == "Modo Busqueda")
            {
                List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
                {
                    cbxAuxiliar,txtNomenclatura,txtDesde,txtHasta,txtNextNCF,txtDisponibles,SinComprobantes
                };
                Clases.ClassControl.ActivadorControlesReadonly(null, true, false, false, listaControles);

                setDatos(0, null);

                List<String> ListName = new List<String>() //Estos son los campos que saldran en la ventana de busqueda, solo si se le pasa esta lista de no ser asi, se mostrarian todos
                {
                    "Proveedor ID","ITBIS","Nomenclatura","Desde","Direccion","Activo"
                };

                SetEnabledButton("Modo Consulta");

                if (tabla.Rows.Count > 1)
                {
                    Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, tabla, ListName);
                    frm.ShowDialog();

                    if (frm.GridMuestra.SelectedItem != null)
                    {
                        DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                        txtComprobanteID.Text = item.Row.ItemArray[0].ToString();
                        setDatos(0, txtComprobanteID.Text);
                        frm.Close();
                    }
                    else
                    {
                        setDatos(0, last);
                    }
                }
                else if (tabla.Rows.Count < 1)
                {
                    BtnProximoRegistro.IsEnabled = false;
                    BtnAnteriorRegistro.IsEnabled = false;
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                    {
                        var message = "No se encontraron datos";
                        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    }
                }

            }
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            SetEnabledButton("Modo Agregar");
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            SetEnabledButton("Modo Editar");
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            SetEnabledButton("Modo Consulta");
            this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            SetControls(false, "Validador", false);
            if (Lista != "Debe Completar los Campos: ")
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(Lista);
                frm.ShowDialog();
            }
            else
            {
                if (Estado == "Modo Editar")
                {
                    setDatos(2, null);
                }
                else
                {
                    setDatos(1, null);
                }
                SetEnabledButton("Modo Consulta");
                setDatos(0, txtComprobanteID.Text);
            }
        }

        private void txtNextNCF_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    txtDisponibles.Text = (Convert.ToDouble(txtHasta.Text) - Convert.ToDouble(txtDesde.Text) - 1).ToString();
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtComprobanteID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtNomenclatura_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtDesde_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtDesde_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtHasta_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtHasta_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    if (txtDesde.Text != null && txtHasta.Text != null && txtDesde.Text != "" && txtHasta.Text != "")
                    {
                        if (Convert.ToInt32(txtDesde.Text) > Convert.ToInt32(txtHasta.Text))
                        {
                            if (SnackbarThree.MessageQueue is { } messageQueue)
                            {
                                Task.Factory.StartNew(() => messageQueue.Enqueue("La cantidad hasta de comprobantes es menor que desde"));
                            }
                        }
                        else
                        {
                            txtDisponibles.Text = (Convert.ToInt32(txtHasta.Text) - Convert.ToInt32(txtDesde.Text)).ToString();
                            if (txtDesde.Text.Length < 8)
                            {
                                string resultado = "";
                                int CantLen = txtDesde.Text.Length;
                                int Total = 8;
                                while (Total > CantLen)
                                {
                                    resultado += "0";
                                    Total--;
                                }
                                txtNextNCF.Text = (txtNomenclatura.Text + resultado.ToString() + txtDesde.Text).ToString();
                            }
                            else if (txtDesde.Text.Length == 8)
                            {
                                txtNextNCF.Text = txtNomenclatura.Text + txtDesde.Text;
                            }
                        }
                    }
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtDireccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtCorreoElectronico_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtTelefono_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtTelefono_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtCelular_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtCelular_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void SinComprobantes_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
                if (e.Key == Key.Enter)
                    ((CheckBox)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void cbxTipoTransaccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void cbxAuxiliar_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void dtpFechaTransaccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    txtNomenclatura.Focus();                    //((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        void setDatos(int Flag, string Transaccion) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            if (Transaccion == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
            {
                if (txtComprobanteID.Text == "")
                {
                    TransaccionID = 0;
                }
                else
                {
                    try
                    {
                        TransaccionID = Convert.ToInt32(txtComprobanteID.Text);
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //Enviamos la excepcion que nos brinda el sistema en caso de que no pueda convertir el id del Proveedor
                    }
                }
            }
            else //Si pasamos un Proveedor, lo convertimos actualizamos la variable Proveedor principal
            {
                TransaccionID = Convert.ToInt32(Transaccion);
            }

            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el Nomenclatura en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@ComprobanteID",TransaccionID),
                new SqlParameter("@Nombre",txtNombre.Text),
                new SqlParameter("@Auxiliar",cbxAuxiliar.Text),
                new SqlParameter("@Nomenclatura",txtNomenclatura.Text),
                new SqlParameter("@Desde",txtDesde.Text),
                new SqlParameter("@Hasta",txtHasta.Text),
                new SqlParameter("@NextNCF",txtNextNCF.Text),
                new SqlParameter("@Disponibles",txtDisponibles.Text),
                new SqlParameter("@UsuarioID",ClassVariables.UsuarioID),
                new SqlParameter("@SinComprobantes",SinComprobantes.IsChecked)
            };

            tabla = Clases.ClassData.runDataTable("sp_conComprobantes", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                frm.ShowDialog();
                ClassVariables.GetSetError = null;
            }

            if (tabla.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
            {
                txtComprobanteID.Text = tabla.Rows[0]["ComprobanteID"].ToString();
                txtNombre.Text = tabla.Rows[0]["Nombre"].ToString();
                cbxAuxiliar.Text = tabla.Rows[0]["Auxiliar"].ToString();
                txtNomenclatura.Text = tabla.Rows[0]["Nomenclatura"].ToString();
                txtDesde.Text = tabla.Rows[0]["Desde"].ToString();
                txtHasta.Text = tabla.Rows[0]["Hasta"].ToString();
                txtNextNCF.Text = tabla.Rows[0]["NextNCF"].ToString();
                txtDisponibles.Text = tabla.Rows[0]["Disponibles"].ToString();
                SinComprobantes.IsChecked = Convert.ToBoolean(tabla.Rows[0]["SinComprobantes"].ToString());

                if (Flag == -1) //si pulsamos el boton del ultimo registro se ejecuta el flag -1 es decir que tenemos una busqueda especial
                {
                    try
                    {
                        LastTransaccionID = Convert.ToInt32(txtComprobanteID.Text); //intentamos convertir el id del Proveedor
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //si presenta un error al intentar convertirlo lo enviamos
                    }
                }
                //ClassControl.setValidador("select * from TsupProveedores where ProveedorID =", txtProveedorID, tbxProveedorID); //ejecutamos el metodo validador con el campo seleccionado para que lo busque y muestre una vez se guarde el registro
            }
            //else
            //{
            //    if (SnackbarThree.MessageQueue is { } messageQueue)
            //    {
            //        var message = "No se encontraron datos";
            //        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            //    }
            //}
            listSqlParameter.Clear(); //Limpiamos la lista de parametros.
        }

        void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
            {
                txtComprobanteID,txtNombre,cbxAuxiliar,txtNomenclatura,txtDesde,txtHasta,txtNextNCF,txtDisponibles,SinComprobantes
            };

            List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
            {
                cbxAuxiliar,txtNomenclatura,txtDesde,txtHasta,txtNextNCF,txtDisponibles,SinComprobantes
            };




            if (Modo == null) //si no trae ningun modo entra el validador
            {
                if (Estado == "Modo Busqueda")
                {
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, listaControles);
                }
                else if (Estado == "Modo Agregar")
                {
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, true, null);
                }
                else
                {
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, null);
                }
            }
            else if (Modo == "Validador") //si el parametro modo es igual a validador ingresa.
            {
                List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que validaremos al dar click en el boton guardar.
                {
                    txtComprobanteID,txtNombre,cbxAuxiliar
                };

                List<Control> listaControlesValidarV2 = new List<Control>() //Estos son los controles que validaremos al dar click en el boton guardar.
                {
                    txtNomenclatura,txtDesde,txtHasta,txtNextNCF,txtDisponibles
                };

                if (!ValidateSaveSinComprobante())
                    listaControlesValidar.AddRange(listaControlesValidarV2);

                Lista = Clases.ClassControl.ValidadorControles(listaControlesValidar); //Este metodo se encarga de validar que cada unos de los controles que se les indica en la lista no se dejen vacios.
                listaControlesValidar.Clear();
            }
            listaControl.Clear(); //limpiamos ambas listas
            listaControles.Clear();
        }

        void SetEnabledButton(String status) //Este metodo se encarga de crear la interacion de los botones de la ventana segun el estado en el que se encuentra
        {

            Estado = status;
            lIconEstado.ToolTip = Estado;

            if (Estado != "Modo Agregar" && Estado != "Modo Editar") //Si el sistema se encuentra en modo consulta o busqueda entra el validador
            {
                BtnPrimerRegistro.IsEnabled = true;
                BtnAnteriorRegistro.IsEnabled = true;
                BtnProximoRegistro.IsEnabled = true;
                BtnUltimoRegistro.IsEnabled = true;
                BtnBuscar.IsEnabled = true;
                BtnImprimir.IsEnabled = true;
                BtnAgregar.IsEnabled = true;
                BtnEditar.IsEnabled = true;

                BtnCancelar.IsEnabled = false;
                BtnGuardar.IsEnabled = false;
                if (Estado == "Modo Consulta") //Si el estado es modo consulta enviamos a ejecutar otro metodo parametizado de forma especial
                {
                    SetControls(true, null, false);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
                }
                else //Si el estado es modo busqueda enviamos a ejecutar el mismo metodo parametizado de forma especial y cambiamos el estado de los botones
                {
                    BtnProximoRegistro.IsEnabled = false;
                    BtnAnteriorRegistro.IsEnabled = false;
                    BtnImprimir.IsEnabled = false;
                    BtnEditar.IsEnabled = false;
                    SetControls(false, null, false);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Search;
                }
            }
            else  //Si el sistema se encuentra en modo Agregar o Editar entra el validador
            {
                BtnPrimerRegistro.IsEnabled = false;
                BtnAnteriorRegistro.IsEnabled = false;
                BtnProximoRegistro.IsEnabled = false;
                BtnUltimoRegistro.IsEnabled = false;

                BtnBuscar.IsEnabled = false;
                BtnImprimir.IsEnabled = false;

                BtnAgregar.IsEnabled = false;
                BtnEditar.IsEnabled = false;

                BtnCancelar.IsEnabled = true;
                BtnGuardar.IsEnabled = true;
                if (Estado == "Modo Agregar") //Si el estado es modo Agregar enviamos a ejecutar otro metodo parametizado de forma especial
                {
                    SetControls(false, null, false);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.AddThick;
                    txtComprobanteID.Text = (LastTransaccionID + 1).ToString();
                }
                else //Si el estado es modo Editar enviamos a ejecutar el mismo metodo parametizado de forma especial
                {
                    SetControls(true, null, true);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
                }
                txtComprobanteID.IsReadOnly = true;
            }
            if (Imprime == false)
            {
                BtnImprimir.IsEnabled = Imprime;
            }
            if (Agrega == false)
            {
                BtnAgregar.IsEnabled = Agrega;
            }
            if (Modifica == false)
            {
                BtnEditar.IsEnabled = Modifica;
            }
        }
        private bool ValidateSaveSinComprobante()
        {
            if (Estado == "Modo Agregar" || Estado == "Modo Editar")
            {
                SqlDataReader MetodoCaja = Clases.ClassData.runSqlDataReader("select * from TconComprobantes where SinComprobantes = 1 and comprobanteid <> " + txtComprobanteID.Text, null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

                if (MetodoCaja.HasRows && MetodoCaja.Read())
                {
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                        Task.Factory.StartNew(() => messageQueue.Enqueue("Ya existe un comprobante con esta opcion habilitada"));
                    SinComprobantes.IsChecked = false;
                    MetodoCaja.Close();
                    return false;
                }
                else
                {
                    MetodoCaja.Close();
                    return SinComprobantes.IsChecked == true ? true : false;
                }
            }
            return false;

        }

        private void SinComprobantes_Checked(object sender, RoutedEventArgs e)
        {
            ValidateSaveSinComprobante();
        }
    }
}
