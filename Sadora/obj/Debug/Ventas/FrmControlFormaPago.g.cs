﻿#pragma checksum "..\..\..\Ventas\FrmControlFormaPago.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7E2872073141A68B2297C1F2A511EFD882DEC317676581B4AC00AA1DCE301027"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Xpf.DXBinding;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.ConditionalFormatting;
using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.Grid.TreeList;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using Sadora.Administracion;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Sadora.Ventas {
    
    
    /// <summary>
    /// FrmControlFormaPago
    /// </summary>
    public partial class FrmControlFormaPago : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.DialogHost MiniDialogo;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lMonto;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem TabItem;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lMontoDistribuido;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMontoDistribuido;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lMontoRestante;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMontoRestante;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Card PanelOpcionesPagos;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WrapPanel PanelWrap;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid BtAceptarCancelar;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAceptar;
        
        #line default
        #line hidden
        
        
        #line 155 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancelar;
        
        #line default
        #line hidden
        
        
        #line 166 "..\..\..\Ventas\FrmControlFormaPago.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Snackbar SnackbarThree;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Sadora;component/ventas/frmcontrolformapago.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Ventas\FrmControlFormaPago.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 12 "..\..\..\Ventas\FrmControlFormaPago.xaml"
            ((Sadora.Ventas.FrmControlFormaPago)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MiniDialogo = ((MaterialDesignThemes.Wpf.DialogHost)(target));
            return;
            case 3:
            this.lMonto = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.TabItem = ((System.Windows.Controls.TabItem)(target));
            return;
            case 5:
            this.lMontoDistribuido = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.txtMontoDistribuido = ((System.Windows.Controls.TextBox)(target));
            
            #line 90 "..\..\..\Ventas\FrmControlFormaPago.xaml"
            this.txtMontoDistribuido.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtMontoDistribuido_KeyDown);
            
            #line default
            #line hidden
            
            #line 90 "..\..\..\Ventas\FrmControlFormaPago.xaml"
            this.txtMontoDistribuido.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtMontoDistribuido_KeyUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lMontoRestante = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.txtMontoRestante = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.PanelOpcionesPagos = ((MaterialDesignThemes.Wpf.Card)(target));
            
            #line 108 "..\..\..\Ventas\FrmControlFormaPago.xaml"
            this.PanelOpcionesPagos.LostFocus += new System.Windows.RoutedEventHandler(this.PanelOpcionesPagos_LostFocus);
            
            #line default
            #line hidden
            return;
            case 10:
            this.PanelWrap = ((System.Windows.Controls.WrapPanel)(target));
            return;
            case 11:
            this.BtAceptarCancelar = ((System.Windows.Controls.Grid)(target));
            return;
            case 12:
            this.btnAceptar = ((System.Windows.Controls.Button)(target));
            
            #line 146 "..\..\..\Ventas\FrmControlFormaPago.xaml"
            this.btnAceptar.Click += new System.Windows.RoutedEventHandler(this.btnAceptar_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.btnCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 155 "..\..\..\Ventas\FrmControlFormaPago.xaml"
            this.btnCancelar.Click += new System.Windows.RoutedEventHandler(this.btnCancelar_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.SnackbarThree = ((MaterialDesignThemes.Wpf.Snackbar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

