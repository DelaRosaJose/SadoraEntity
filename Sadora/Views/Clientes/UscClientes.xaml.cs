using Sadora.Clases;
using Sadora.ViewModels.Clientes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.Clientes
{
    public partial class UscClientes : UserControl
    {
        ViewModels.Clientes.ClientesViewModel Cli = new ViewModels.Clientes.ClientesViewModel();
        public UscClientes()
        {
            InitializeComponent();
            Name = nameof(UscClientes);
            this.DataContext = Cli;

        }

        bool Imprime, Modifica, Agrega;
        bool PuedeUsarBotonAnular = false;

        bool Inicializador = false;
        DataTable tabla;
        SqlDataReader reader;
        string Estado, Lista, last;
        int ClienteID, LastClienteID;
        private int? _FistClienteID, _LastClienteID;


        private void UserControl_Initialized(object sender, EventArgs e) => Inicializador = true;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Inicializador == true)
            {
                Inicializador = false;
                Imprime = ClassVariables.Imprime;
                Agrega = ClassVariables.Agrega;
                Modifica = ClassVariables.Modifica;
                Task.Run(() =>
                {
                    using (Models.SadoraEntities db = new Models.SadoraEntities())
                        _FistClienteID = db.TcliClientes.FirstOrDefault().ClienteID;
                });//busqueda de Primer Registro

                this.ControlesGenerales.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void UscBotones_Click(object sender, RoutedEventArgs e)
        {
            int intValue;
            using (Models.SadoraEntities db = new Models.SadoraEntities())
            {
                string ButtonName = ((Button)e.OriginalSource).Name;

                switch (ButtonName)
                {
                    case "BtnPrimerRegistro":
                        Cli.Cliente = db.TcliClientes.OrderBy(x => x.ClienteID).FirstOrDefault();
                        _FistClienteID = Cli.Cliente.ClienteID;
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        break;

                    case "BtnAnteriorRegistro":
                        intValue = int.TryParse(Cli.Cliente.ClienteID.ToString(), out intValue) ? intValue : 0;
                        Cli.Cliente = db.TcliClientes.Where(x => x.ClienteID == (intValue - 1)).OrderBy(x => x.ClienteID).FirstOrDefault();
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: Cli.Cliente.ClienteID > _FistClienteID ? ButtonName : "BtnPrimerRegistro");
                        break;

                    case "BtnProximoRegistro":
                        intValue = int.TryParse(Cli.Cliente.ClienteID.ToString(), out intValue) ? intValue : 0;
                        Cli.Cliente = db.TcliClientes.Where(x => x.ClienteID == (intValue + 1)).OrderBy(x => x.ClienteID).FirstOrDefault();
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: Cli.Cliente.ClienteID < _LastClienteID ? ButtonName : "BtnUltimoRegistro");
                        break;

                    case "BtnUltimoRegistro":
                        Cli.Cliente = db.TcliClientes.OrderByDescending(x => x.ClienteID).FirstOrDefault();
                        _LastClienteID = Cli.Cliente.ClienteID;
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        break;
                    case "BtnBuscar":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        break;
                    case "BtnAgregar":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        LimpiadorGeneral(MainView.Children);
                        break;
                    case "BtnEditar":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        break;
                    case "BtnCancelar":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: "BtnUltimoRegistro");
                        break;
                    case "BtnGuardar":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: "BtnUltimoRegistro");
                        break;
                }
            }

            if (Imprime == false)
                ControlesGenerales.BtnImprimir.IsEnabled = Imprime;
            if (Agrega == false)
                ControlesGenerales.BtnAgregar.IsEnabled = Agrega;
            if (Modifica == false)
                ControlesGenerales.BtnEditar.IsEnabled = Modifica;
            if (PuedeUsarBotonAnular == false)
                ControlesGenerales.BtnAnular.IsEnabled = PuedeUsarBotonAnular;

            Estado = ControlesGenerales.EstadoVentana;
        }



        void LimpiadorGeneral(UIElementCollection view)
        {
            for (int i = 0; i < view.Count; i++)
            {
                var Elemente = view[i].GetType();
                var Elementv = view[i];

                if (view[i] is Grid)
                    LimpiadorGeneral((view[i] as Grid).Children);
                //else if (view[i] is TextBox)
                //{
                //    var Element = view[i] as TextBox;
                //}
                else if (view[i] is ScrollViewer && (view[i] as ScrollViewer).Content is Grid)
                    LimpiadorGeneral(((view[i] as ScrollViewer).Content as Grid).Children);
                else if (view[i] is StackPanel)
                    LimpiadorGeneral((view[i] as StackPanel).Children);
                else if (view[i] is CustomElements.UscTextboxGeneral)
                    (view[i] as CustomElements.UscTextboxGeneral).Text = string.Empty;

            }
            //var Childs1 = view.;


            //Control Control;

            //Control = view;

            //var result = Control.GetType();

            //if (view is UserControl)
            //{
            //    //Control = Control as UserControl;//.IsReadOnly = !funcion;
            //    //var result1 = Control.GetType();

            //    var result4 = (view as UserControl).Content;
            //    foreach (var item in (view as UserControl))
            //    {

            //    }
            //}
            //if (view is TextBox)
            //    view.tex

        }




        //private void BtnPrimerRegistro_Click(object sender, RoutedEventArgs e)
        //{
        //    //List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
        //    //{
        //    //   txtRNC,txtNombre
        //    //};
        //    //ClassControl.ClearControl(listaControl);
        //    SetEnabledButton("Modo Consulta");
        //    //setDatos(0, "1");
        //    //BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = false;
        //    //lstv2 = lst.FirstOrDefault();
        //    //this.DataContext = lst.FirstOrDefault();
        //    var ew = Cli;
        //}

        //private void BtnAnteriorRegistro_Click(object sender, RoutedEventArgs e)
        //{
        //    //List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
        //    //{
        //    //   txtRNC,txtNombre
        //    //};
        //    //ClassControl.ClearControl(listaControl);
        //    //SetEnabledButton("Modo Consulta");
        //    //try
        //    //{
        //    //    ClienteID = Convert.ToInt32(txtClienteID.Text) - 1;
        //    //}
        //    //catch (Exception exception)
        //    //{
        //    //    ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
        //    //}


        //    //if (ClienteID <= 1)
        //    //{
        //    //    BtnPrimerRegistro.IsEnabled = false;
        //    //    BtnAnteriorRegistro.IsEnabled = false;
        //    //    setDatos(0, "1");
        //    //}
        //    //else
        //    //{
        //    //    setDatos(0, ClienteID.ToString());
        //    //}
        //    SetEnabledButton("Modo Consulta");
        //    //BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = false;
        //    //this.DataContext = lst.FirstOrDefault();
        //}

        //private void BtnProximoRegistro_Click(object sender, RoutedEventArgs e)
        //{
        //    //List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
        //    //{
        //    //   txtRNC,txtNombre
        //    //};
        //    //ClassControl.ClearControl(listaControl);
        //    //SetEnabledButton("Modo Consulta");

        //    //txtClienteID.Text += "p";
        //    //int result = ClassControl.TryCastInt(txtClienteID.Text) == 0 ? ClienteID : ClassControl.TryCastInt(txtClienteID.Text) + 1;
        //    //ClienteID = ClassControl.TryCastInt(txtClienteID.Text) == 0 ? ClienteID : ClienteID + 1;//Convert.ToInt32(txtClienteID.Text) + 1;

        //    //try
        //    //{
        //    //    ClienteID = Convert.ToInt32(txtClienteID.Text) + 1;
        //    //}
        //    //catch (Exception exception)
        //    //{
        //    //    ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
        //    //}

        //    //if (ClienteID >= LastClienteID)
        //    //{
        //    //    BtnUltimoRegistro.IsEnabled = false;
        //    //    BtnProximoRegistro.IsEnabled = false;
        //    //    setDatos(0, LastClienteID.ToString());
        //    //}
        //    //else
        //    //{
        //    //    setDatos(0, ClienteID.ToString());
        //    //}

        //    SetEnabledButton("Modo Consulta");
        //    //var result = this.DataContext;

        //}

        //private void BtnUltimoRegistro_Click(object sender, RoutedEventArgs e)
        //{
        //    //List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
        //    //{
        //    //   txtRNC,txtNombre
        //    //};
        //    //ClassControl.ClearControl(listaControl);
        //    //SetEnabledButton("Modo Consulta");
        //    //setDatos(-1, "1");
        //    SetEnabledButton("Modo Consulta");
        //    //BtnUltimoRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = false;
        //}

        //private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        //{

        //    if (Estado == "Modo Consulta")
        //    {
        //        last = txtClienteID.Text;
        //        SetEnabledButton("Modo Busqueda");
        //    }
        //    else if (Estado == "Modo Busqueda")
        //    {
        //        List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
        //        {
        //            txtRepresentante,txtClaseID,tbxClaseID,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular,cActivar
        //        };
        //        Clases.ClassControl.ActivadorControlesReadonly(null, true, false, false, listaControles);

        //        setDatos(0, null);

        //        List<String> ListName = new List<String>() //Estos son los campos que saldran en la ventana de busqueda, solo si se le pasa esta lista de no ser asi, se mostrarian todos
        //        {
        //            "Cliente ID","RNC","Nombre","Representante","Direccion","Activo"
        //        };

        //        SetEnabledButton("Modo Consulta");

        //        if (tabla.Rows.Count > 1)
        //        {
        //            Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, tabla, ListName);
        //            frm.ShowDialog();

        //            if (frm.GridMuestra.SelectedItem != null)
        //            {
        //                DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
        //                txtClienteID.Text = item.Row.ItemArray[0].ToString();
        //                setDatos(0, txtClienteID.Text);
        //                frm.Close();
        //            }
        //            else
        //            {
        //                setDatos(0, last);
        //            }
        //        }
        //        else if (tabla.Rows.Count < 1)
        //        {
        //            //BtnProximoRegistro.IsEnabled = false;
        //            //BtnAnteriorRegistro.IsEnabled = false;

        //            if (SnackbarThree.MessageQueue is { } messageQueue)
        //            {
        //                var message = "No se encontraron datos";
        //                Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        //            }
        //        }

        //    }
        //}

        //private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        //    SetEnabledButton("Modo Agregar");
        //}

        //private void BtnEditar_Click(object sender, RoutedEventArgs e)
        //{
        //    SetEnabledButton("Modo Editar");
        //}

        //private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        //{
        //    SetEnabledButton("Modo Consulta");
        //    //this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        //}

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            /*    SetControls(false, "Validador", false);
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
                        SetEnabledButton("Modo Consulta");
                        setDatos(0, txtClienteID.Text);
                    }
                    else
                    {
                        SqlDataReader tabla = ClassControl.getDatosCedula(txtRNC.Text);
                        if (tabla != null)
                        {
                            tabla.Close();
                            tabla.Dispose();
                            //if (Estado == "Modo Editar")
                            //    setDatos(2, null);
                            //else
                            setDatos(1, null);

                            SetEnabledButton("Modo Consulta");
                            setDatos(0, txtClienteID.Text);
                        }
                        else if (ClassVariables.ExistClient)
                        {
                            if (SnackbarThree.MessageQueue is { } messageQueue)
                            {
                                var message = "Ya existe un cliente con este RNC";
                                Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                            }
                        }
                    }
                }*/
        }

        private void txtRNC_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    reader = ClassControl.getDatosCedula(Cli.Cliente.RNC);
                    if (reader != null)
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                Cli.Cliente.Nombre = reader["NombreCompleto"].ToString();
                                //txtDireccion.Text = reader["Direccion"].ToString();
                                //txtTelefono.Text = reader["Telefono"].ToString();
                                reader.NextResult();

                            }
                            reader.Close();
                            reader.Dispose();
                        }
                        else
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                        ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                    }
                    else if (ClassVariables.ExistClient)
                    {
                        if (SnackbarThree.MessageQueue is { } messageQueue)
                        {
                            var message = "Ya existe un cliente con este RNC";
                            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                        }
                    }

                }
            }
        }

        private void txtClienteID_KeyUp(object sender, KeyEventArgs e)
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

        private void txtRepresentante_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void btnClaseID_Click(object sender, RoutedEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost("Select ClaseID, Nombre from TcliClaseClientes", null);
                frm.ShowDialog();

                if (frm.GridMuestra.SelectedItem != null)
                {
                    DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                    txtClaseID.Text = item.Row.ItemArray[0].ToString();

                    ClassControl.setValidador("select ClaseID, Nombre from TcliClaseClientes where ClaseID =", txtClaseID, tbxClaseID);
                }
            }
        }

        private void txtClaseID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    if (txtClaseID.Text != "")
                        ClassControl.setValidador("select Nombre from TcliClaseClientes where ClaseID =", txtClaseID, tbxClaseID);
                    else
                    {
                        txtClaseID.Text = 0.ToString();
                        ClassControl.setValidador("select Nombre from TcliClaseClientes where ClaseID =", txtClaseID, tbxClaseID);
                    }
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtClaseID_KeyDown(object sender, KeyEventArgs e) => ClassControl.ValidadorNumeros(e);

        private void btnComprobanteID_Click(object sender, RoutedEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost("Select ComprobanteID, Nombre, Auxiliar, NextNCF, Disponibles from TconComprobantes where Auxiliar = 'Clientes'", null);
                frm.ShowDialog();

                if (frm.GridMuestra.SelectedItem != null)
                {
                    DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                    txtComprobanteID.Text = item.Row.ItemArray[0].ToString();

                    ClassControl.setValidador("select ComprobanteID, Nombre, Auxiliar, NextNCF, Disponibles from TconComprobantes where Auxiliar = 'Clientes' and ComprobanteID =", txtComprobanteID, tbxComprobanteID);
                }
            }
        }

        private void txtComprobanteID_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtComprobanteID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    if (txtComprobanteID.Text != "")
                        ClassControl.setValidador("select Nombre from TconComprobantes where Auxiliar = 'Clientes' and ComprobanteID =", txtComprobanteID, tbxComprobanteID);
                    else
                    {
                        txtComprobanteID.Text = 0.ToString();
                        ClassControl.setValidador("select Nombre from TconComprobantes where Auxiliar = 'Clientes' and ComprobanteID =", txtComprobanteID, tbxComprobanteID);
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

        private void StackPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((StackPanel)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));

        }

        private void UscTextboxGeneral_KeyDown(object sender, KeyEventArgs e)
        {
            if (Estado == "Modo Consulta")
                e.Handled = true;

            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtCelular_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));

        }

        private void txtCelular_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void cActivar_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((CheckBox)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        //void setDatos(int Flag, string Cliente) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        //{
        //    //if (Cliente == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
        //    //{
        //    //    if (txtClienteID.Text == "")
        //    //        ClienteID = 0;
        //    //    else
        //    //    {
        //    //        //ClienteID = ClassControl.TryCastInt(txtClienteID.Text) == 0 ? ClienteID : ClienteID + 1;
        //    //        try
        //    //        {
        //    //            ClienteID = Convert.ToInt32(txtClienteID.Text);
        //    //        }
        //    //        catch (Exception exception)
        //    //        {
        //    //            ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //Enviamos la excepcion que nos brinda el sistema en caso de que no pueda convertir el id del cliente
        //    //        }
        //    //    }
        //    //}
        //    //else //Si pasamos un cliente, lo convertimos actualizamos la variable cliente principal
        //    //{
        //    //    //ClienteID = ClassControl.TryCastInt(Cliente) == 0 ? ClienteID : ClienteID + 1;
        //    //    ClienteID = Convert.ToInt32(Cliente);
        //    //}

        //    //List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
        //    //{
        //    //    new SqlParameter("Flag",Flag),
        //    //    new SqlParameter("@ClienteID",ClienteID),
        //    //    new SqlParameter("@RNC",txtRNC.Text),
        //    //    new SqlParameter("@Nombre",txtNombre.Text),
        //    //    new SqlParameter("@Representante",txtRepresentante.Text),
        //    //    new SqlParameter("@ClaseID",txtClaseID.Text),
        //    //    new SqlParameter("@ClaseComprobanteID",txtComprobanteID.Text),
        //    //    new SqlParameter("@DiasCredito",txtDiasCredito.Text),
        //    //    new SqlParameter("@Direccion",txtDireccion.Text),
        //    //    new SqlParameter("@CorreoElectronico",txtCorreoElectronico.Text),
        //    //    new SqlParameter("@Telefono",txtTelefono.Text),
        //    //    new SqlParameter("@Celular",txtCelular.Text),
        //    //    new SqlParameter("@Activo",cActivar.IsChecked),
        //    //    new SqlParameter("@UsuarioID",ClassVariables.UsuarioID)
        //    //};

        //    //tabla = Clases.ClassData.runDataTable("sp_cliclientes", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

        //    //if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
        //    //{
        //    //    Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
        //    //    frm.ShowDialog();
        //    //    ClassVariables.GetSetError = null;
        //    //}

        //    //if (tabla.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
        //    //{
        //    //    txtClienteID.Text = tabla.Rows[0]["ClienteID"].ToString();
        //    //    txtRNC.Text = tabla.Rows[0]["RNC"].ToString();
        //    //    txtNombre.Text = tabla.Rows[0]["Nombre"].ToString();
        //    //    txtRepresentante.Text = tabla.Rows[0]["Representante"].ToString();
        //    //    txtClaseID.Text = tabla.Rows[0]["ClaseID"].ToString();
        //    //    txtComprobanteID.Text = tabla.Rows[0]["ClaseComprobanteID"].ToString();
        //    //    txtDireccion.Text = tabla.Rows[0]["Direccion"].ToString();
        //    //    txtCorreoElectronico.Text = tabla.Rows[0]["CorreoElectronico"].ToString();
        //    //    txtTelefono.Text = tabla.Rows[0]["Telefono"].ToString();
        //    //    txtCelular.Text = tabla.Rows[0]["Celular"].ToString();
        //    //    cActivar.IsChecked = Convert.ToBoolean(tabla.Rows[0]["Activo"].ToString());
        //    //    txtDiasCredito.Text = tabla.Rows[0]["DiasCredito"].ToString();

        //    //    if (Flag == -1) //si pulsamos el boton del ultimo registro se ejecuta el flag -1 es decir que tenemos una busqueda especial
        //    //    {
        //    //        try
        //    //        {
        //    //            LastClienteID = Convert.ToInt32(txtClienteID.Text); //intentamos convertir el id del cliente
        //    //        }
        //    //        catch (Exception exception)
        //    //        {
        //    //            ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //si presenta un error al intentar convertirlo lo enviamos
        //    //        }
        //    //    }
        //    //    ClassControl.setValidador("select * from TcliClaseClientes where ClaseID =", txtClaseID, tbxClaseID); //ejecutamos el metodo validador con el campo seleccionado para que lo busque y muestre una vez se guarde el registro
        //    //    ClassControl.setValidador("select * from TconComprobantes where ComprobanteID =", txtComprobanteID, tbxComprobanteID); //ejecutamos el metodo validador con el campo seleccionado para que lo busque y muestre una vez se guarde el registro
        //    //}
        //    //listSqlParameter.Clear(); //Limpiamos la lista de parametros.
        //}

        //void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        //{
        //    List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
        //    {
        //        txtClienteID,txtRNC,txtNombre,txtRepresentante,txtClaseID,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular,cActivar,txtDiasCredito
        //    };

        //    List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
        //    {
        //        txtRepresentante,txtClaseID,tbxClaseID,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular,cActivar,txtDiasCredito
        //    };

        //    List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que validaremos al dar click en el boton guardar.
        //    {
        //        txtClienteID,txtRNC,txtNombre,txtRepresentante//,txtClaseID,tbxClaseID,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular
        //    };

        //    if (Modo == null) //si no trae ningun modo entra el validador
        //    {
        //        if (Estado == "Modo Busqueda")
        //        {
        //            Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, listaControles);
        //        }
        //        else if (Estado == "Modo Agregar")
        //        {
        //            Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, true, null);
        //        }
        //        else
        //        {
        //            Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, null);
        //        }
        //    }
        //    else if (Modo == "Validador") //si el parametro modo es igual a validador ingresa.
        //    {
        //        Lista = Clases.ClassControl.ValidadorControles(listaControlesValidar); //Este metodo se encarga de validar que cada unos de los controles que se les indica en la lista no se dejen vacios.
        //    }
        //    listaControl.Clear(); //limpiamos ambas listas
        //    listaControles.Clear();
        //    listaControlesValidar.Clear();
        //}

        //void SetEnabledButton(String status) //Este metodo se encarga de crear la interacion de los botones de la ventana segun el estado en el que se encuentra
        //{
        //    Estado = status;
        //    //lIconEstado.ToolTip = Estado;

        //    if (Estado != "Modo Agregar" && Estado != "Modo Editar") //Si el sistema se encuentra en modo consulta o busqueda entra el validador
        //    {
        //        //BtnPrimerRegistro.IsEnabled = true;
        //        //BtnAnteriorRegistro.IsEnabled = true;
        //        //BtnProximoRegistro.IsEnabled = true;
        //        //BtnUltimoRegistro.IsEnabled = true;
        //        //BtnBuscar.IsEnabled = true;
        //        //BtnImprimir.IsEnabled = true;
        //        //BtnAgregar.IsEnabled = true;
        //        //BtnEditar.IsEnabled = true;

        //        //BtnCancelar.IsEnabled = false;
        //        //BtnGuardar.IsEnabled = false;
        //        if (Estado == "Modo Consulta") //Si el estado es modo consulta enviamos a ejecutar otro metodo parametizado de forma especial
        //        {
        //            SetControls(true, null, false);
        //            //IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
        //        }
        //        else //Si el estado es modo busqueda enviamos a ejecutar el mismo metodo parametizado de forma especial y cambiamos el estado de los botones
        //        {
        //            //BtnProximoRegistro.IsEnabled = false;
        //            //BtnAnteriorRegistro.IsEnabled = false;
        //            //BtnImprimir.IsEnabled = false;
        //            //BtnEditar.IsEnabled = false;
        //            SetControls(false, null, false);
        //            //IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Search;
        //        }
        //    }
        //    else  //Si el sistema se encuentra en modo Agregar o Editar entra el validador
        //    {
        //        //BtnPrimerRegistro.IsEnabled = false;
        //        //BtnAnteriorRegistro.IsEnabled = false;
        //        //BtnProximoRegistro.IsEnabled = false;
        //        //BtnUltimoRegistro.IsEnabled = false;

        //        //BtnBuscar.IsEnabled = false;
        //        //BtnImprimir.IsEnabled = false;

        //        //BtnAgregar.IsEnabled = false;
        //        //BtnEditar.IsEnabled = false;

        //        //BtnCancelar.IsEnabled = true;
        //        //BtnGuardar.IsEnabled = true;
        //        if (Estado == "Modo Agregar") //Si el estado es modo Agregar enviamos a ejecutar otro metodo parametizado de forma especial
        //        {
        //            SetControls(false, null, false);
        //            //IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.AddThick;
        //            txtClienteID.Text = (LastClienteID + 1).ToString();
        //            txtRNC.Focus();
        //        }
        //        else //Si el estado es modo Editar enviamos a ejecutar el mismo metodo parametizado de forma especial
        //        {
        //            SetControls(true, null, true);
        //            //IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
        //        }
        //        txtClienteID.IsReadOnly = true;
        //    }
        //    //if (Imprime == false)
        //    //    BtnImprimir.IsEnabled = Imprime;
        //    //if (Agrega == false)
        //    //    BtnAgregar.IsEnabled = Agrega;
        //    //if (Modifica == false)
        //    //    BtnEditar.IsEnabled = Modifica;
        //}

        private void txtDiasCredito_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtDiasCredito_KeyDown(object sender, KeyEventArgs e) => ClassControl.ValidadorNumeros(e);

    }
}
