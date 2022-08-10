using Sadora.Clases;
using Sadora.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Sadora.Administracion
{
    /// <summary>
    /// Lógica de interacción para UscEmpresa.xaml
    /// </summary>
    public partial class UscEmpresa : UserControl
    {
        ViewModels.Administracion.EmpresaViewModel ViewModel = new ViewModels.Administracion.EmpresaViewModel();

        bool Imprime, Modifica, Agrega;
        bool PuedeUsarBotonAnular = false;

        public bool Inicializador = false;
        DataTable tabla;
        string Estado;
        string Lista;
        int ID;
        int LastID;
        //string last = "";

        int? _FistID, _LastID, last;

        byte[] pic = null;

        public UscEmpresa()
        {
            InitializeComponent();
            Name = nameof(UscEmpresa);
            this.DataContext = ViewModel;
        }

        private void UserControl_Initialized(object sender, EventArgs e) => Inicializador = true;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Inicializador == true)
            {
                Inicializador = false;
                Imprime = ClassVariables.Imprime;
                Agrega = ClassVariables.Agrega;
                Modifica = ClassVariables.Modifica;

                ViewModel.EstadoVentana = "Modo Consulta";
                this.ControlesGenerales.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private async void UscBotones_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int intValue;
                using (SadoraEntity db = new Models.SadoraEntity())
                {
                    string ButtonName = ((Button)e.OriginalSource).Name;

                    TsysEmpresa Tsql = default;

                    switch (ButtonName)
                    {
                        case "BtnPrimerRegistro":
                            Tsql = db.TsysEmpresas.OrderBy(x => x.ID).FirstOrDefault();
                            ViewModel.Empresa = Tsql != default ? Tsql : ViewModel.Empresa;
                            _FistID = ViewModel.Empresa.ID;
                            ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                            break;

                        case "BtnAnteriorRegistro":
                            intValue = int.TryParse(ViewModel.Empresa.ID.ToString(), out intValue) ? intValue : 0;
                            Tsql = db.TsysEmpresas.Where(x => x.ID == (intValue - 1)).OrderBy(x => x.ID).FirstOrDefault();
                            ViewModel.Empresa = Tsql != default ? Tsql : ViewModel.Empresa;
                            ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ViewModel.Empresa.ID > _FistID ? ButtonName : "BtnPrimerRegistro");
                            break;

                        case "BtnProximoRegistro":
                            intValue = int.TryParse(ViewModel.Empresa.ID.ToString(), out intValue) ? intValue : 0;
                            Tsql = db.TsysEmpresas.Where(x => x.ID == (intValue + 1)).OrderBy(x => x.ID).FirstOrDefault();
                            ViewModel.Empresa = Tsql != default ? Tsql : ViewModel.Empresa;
                            ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ViewModel.Empresa.ID < _LastID ? ButtonName : "BtnUltimoRegistro");
                            break;

                        case "BtnUltimoRegistro":
                            Tsql = db.TsysEmpresas.OrderByDescending(x => x.ID).FirstOrDefault();
                            ViewModel.Empresa = Tsql != default ? Tsql : ViewModel.Empresa;
                            _LastID = ViewModel.Empresa.ID;
                            ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                            break;

                        case "BtnBuscar":
                            if (ViewModel.EstadoVentana == "Modo Consulta")
                                last = ViewModel.Empresa.ID;
                            else if (ViewModel.EstadoVentana == "Modo Busqueda")
                            {


                                ViewModel.Empresa.ID = 0;
                                var Result = db.TsysEmpresas.Where(x => (x.ID == ViewModel.Empresa.ID || ViewModel.Empresa.ID == 0)

                                                                        //(ClienteID = @ClienteID or @ClienteID = 0) and(RNC = @RNC or @RNC = '') and(Nombre like '%' + @Nombre + '%' or @Nombre = '')

                                                                        /* || (x.RNC.Contains(ViewModel.Cliente.RNC) //== ViewModel.Cliente.RNC
                                                                         && x.Nombre.StartsWith(ViewModel.Cliente.Nombre)// == ViewModel.Cliente.Nombre
                                                                         && x.Representante.Contains(ViewModel.Cliente.Representante)// == ViewModel.Cliente.Representante
                                                                         && x.Direccion.Contains(ViewModel.Cliente.Direccion)// == ViewModel.Cliente.Direccion
                                                                         && x.CorreoElectronico.Contains(ViewModel.Cliente.CorreoElectronico)// == ViewModel.Cliente.CorreoElectronico
                                                                         && x.Telefono.Contains(ViewModel.Cliente.Telefono)// == ViewModel.Cliente.Telefono
                                                                         && x.Celular.Contains(ViewModel.Cliente.Celular))
                                                                        */

                                                                        //&& x.Nombre.Contains(ViewModel.Cliente.Nombre)// == ViewModel.Cliente.Nombre
                                                                        //&& x.Representante.Contains(ViewModel.Cliente.Representante)// == ViewModel.Cliente.Representante
                                                                        //&& x.Direccion.Contains(ViewModel.Cliente.Direccion)// == ViewModel.Cliente.Direccion
                                                                        //&& x.CorreoElectronico.Contains(ViewModel.Cliente.CorreoElectronico)// == ViewModel.Cliente.CorreoElectronico
                                                                        //&& x.Telefono.Contains(ViewModel.Cliente.Telefono)// == ViewModel.Cliente.Telefono
                                                                        //&& x.Celular.Contains(ViewModel.Cliente.Celular))// == ViewModel.Cliente.Celular
                                                                        ).ToList();
                                if (Result.Count == 0)
                                    ClassControl.PresentadorSnackBar(SnackbarThree, "No se encontraron datos");
                                else if (Result.Count == 1)
                                    ViewModel.Empresa = Result[0];
                                else
                                {
                                    Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, ClassControl.ListToDataTable(Result), null);
                                    frm.ShowDialog();

                                    if (frm.GridMuestra.SelectedItem != null)
                                    {
                                        DataRowView item = frm.GridMuestra.SelectedItem as DataRowView;
                                        //txtClienteID.Text = item.Row.ItemArray[0].ToString();
                                        //setDatos(0, txtClienteID.Text);
                                        frm.Close();
                                    }
                                    else
                                    {
                                        //setDatos(0, last);
                                    }
                                }

                                //var matches = from x in db.TcliClientes
                                //              where x.ClienteID.Contains(ViewModel.Cliente.ClienteID)
                                //              select x;


                                //var ResultSet = "";//db.TcliClientes.e
                                //Where(x=>x.ClienteID = ViewModel.Cliente.ClienteID).FirstOrDefault();
                            }

                            //MessageBox.Show(ViewModel.EstadoVentana);

                            //ViewModel.Cliente = db.TcliClientes.Find(ViewModel.Cliente.ClienteID);//Where(x=>x.ClienteID = ViewModel.Cliente.ClienteID).FirstOrDefault();

                            ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName, EstadoVentanaPadre: ViewModel.EstadoVentana);
                            break;

                        case "BtnAgregar":
                            ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                            ClassControl.LimpiadorGeneral(MainView.Children);
                            break;

                        case "BtnEditar":
                            ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                            break;

                        case "BtnCancelar":
                            ControlesGenerales.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                            break;

                        case "BtnGuardar":
                            if (ViewModel.EstadoVentana == "Modo Agregar")
                                db.TsysEmpresas.Add(ViewModel.Empresa);
                            else if (ViewModel.EstadoVentana == "Modo Editar")
                                db.Entry(ViewModel.Empresa).State = System.Data.Entity.EntityState.Modified;

                            await db.SaveChangesAsync();

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

                ViewModel.EstadoVentana = ControlesGenerales.EstadoVentana;

            }
            catch (System.Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }


        #region CodeEvents

        //private void BtnPrimerRegistro_Click(object sender, RoutedEventArgs e)
        //{
        //    List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
        //    {
        //       txtNombre
        //    };
        //    ClassControl.ClearControl(listaControl);
        //    SetEnabledButton("Modo Consulta");
        //    setDatos(0, "1");
        //    BtnPrimerRegistro.IsEnabled = false;
        //    BtnAnteriorRegistro.IsEnabled = false;
        //}

        //private void BtnAnteriorRegistro_Click(object sender, RoutedEventArgs e)
        //{
        //    List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
        //    {
        //       txtNombre
        //    };
        //    ClassControl.ClearControl(listaControl);
        //    SetEnabledButton("Modo Consulta");
        //    try
        //    {
        //        ID = Convert.ToInt32(txtID.Text) - 1;
        //    }
        //    catch (Exception exception)
        //    {
        //        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
        //    }


        //    if (ID <= 1)
        //    {
        //        BtnPrimerRegistro.IsEnabled = false;
        //        BtnAnteriorRegistro.IsEnabled = false;
        //        setDatos(0, "1");
        //    }
        //    else
        //    {
        //        setDatos(0, ID.ToString());
        //    }
        //}

        //private void BtnProximoRegistro_Click(object sender, RoutedEventArgs e)
        //{
        //    List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
        //    {
        //       txtNombre
        //    };
        //    ClassControl.ClearControl(listaControl);
        //    SetEnabledButton("Modo Consulta");
        //    try
        //    {
        //        ID = Convert.ToInt32(txtID.Text) + 1;
        //    }
        //    catch (Exception exception)
        //    {
        //        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
        //    }

        //    if (ID >= LastID)
        //    {
        //        BtnUltimoRegistro.IsEnabled = false;
        //        BtnProximoRegistro.IsEnabled = false;
        //        setDatos(0, LastID.ToString());
        //    }
        //    else
        //    {
        //        setDatos(0, ID.ToString());
        //    }
        //}

        //private void BtnUltimoRegistro_Click(object sender, RoutedEventArgs e)
        //{
        //    List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
        //    {
        //       txtNombre
        //    };
        //    ClassControl.ClearControl(listaControl);
        //    SetEnabledButton("Modo Consulta");
        //    setDatos(-1, "1");
        //    BtnUltimoRegistro.IsEnabled = false;
        //    BtnProximoRegistro.IsEnabled = false;
        //}

        //private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        //{

        //    if (Estado == "Modo Consulta")
        //    {
        //        last = txtID.Text;
        //        SetEnabledButton("Modo Busqueda");
        //    }
        //    else if (Estado == "Modo Busqueda")
        //    {
        //        List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
        //        {
        //            txtID//,tbxEmpleadoID,txtPassword,txtGrupoID,tbxGrupoID,cActivar
        //        };
        //        Clases.ClassControl.ActivadorControlesReadonly(null, true, false, false, listaControles);

        //        setDatos(0, null);

        //        List<String> ListName = new List<String>() //Estos son los campos que saldran en la ventana de busqueda, solo si se le pasa esta lista de no ser asi, se mostrarian todos
        //        {
        //            "Usuario ID","Nombre","Empleado ID","Grupo ID","Activo"
        //        };

        //        SetEnabledButton("Modo Consulta");

        //        if (tabla.Rows.Count > 1)
        //        {
        //            Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, tabla, ListName);
        //            frm.ShowDialog();

        //            if (frm.GridMuestra.SelectedItem != null)
        //            {
        //                DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
        //                txtID.Text = item.Row.ItemArray[0].ToString();
        //                setDatos(0, txtID.Text);
        //                frm.Close();
        //            }
        //            else
        //            {
        //                setDatos(0, last);
        //            }
        //        }
        //        else if (tabla.Rows.Count < 1)
        //        {
        //            BtnProximoRegistro.IsEnabled = false;
        //            BtnAnteriorRegistro.IsEnabled = false;
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
        //    last = txtID.Text;
        //    this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        //    SetEnabledButton("Modo Agregar");
        //}

        //private void BtnEditar_Click(object sender, RoutedEventArgs e)
        //{
        //    SetEnabledButton("Modo Editar");
        //}

        //private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        //{
        //    SetEnabledButton("Modo Consulta");
        //    this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        //}

        //private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        //{
        //    SetControls(false, "Validador", false);
        //    if (Lista != "Debe Completar los Campos: ")
        //    {
        //        Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(Lista);
        //        frm.ShowDialog();
        //    }
        //    else
        //    {
        //        //GridMain.ClearColumnFilter("Visualiza");
        //        if (Estado == "Modo Editar")
        //        {
        //            setDatos(2, null);
        //        }
        //        else
        //        {
        //            Estado = "Modo Agregar";
        //            setDatos(1, null);
        //        }
        //        SetEnabledButton("Modo Consulta");
        //        setDatos(0, txtID.Text);
        //        ClassVariables.ClasesVariables.NombreEmpresa = txtNombre.Text;
        //        ClassVariables.ClasesVariables.RNCEmpresa = txtRNC.Text;
        //        ClassVariables.ClasesVariables.TelefonoEmpresa = txtTelefono.Text;
        //        ClassVariables.ClasesVariables.DireccionEmpresa = txtDireccion.Text;
        //        ClassVariables.LogoEmpresa = pic;
        //    }
        //}

        #endregion
        

        private void BtnCargarFotos_Click(object sender, RoutedEventArgs e)
        {
            if (Estado == "Modo Agregar" || Estado == "Modo Editar")
            {
                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
                dlg.InitialDirectory = "c:\\";
                dlg.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All Files (*.*)|*.*";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFileName = dlg.FileName;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFileName);
                    bitmap.EndInit();
                    ImagePickture.Source = bitmap;

                    Stream myStream = dlg.OpenFile();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        myStream.CopyTo(ms);
                        pic = ms.ToArray();
                    }
                }
            }
        }



    }
}
