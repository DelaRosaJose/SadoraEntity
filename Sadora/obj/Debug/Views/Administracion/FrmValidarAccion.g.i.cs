﻿#pragma checksum "..\..\..\..\Views\Administracion\FrmValidarAccion.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "00CE77B06BDACB3826E69E5DD538F7FA668906A3AB5E9E65531D687C600DBD11"
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


namespace Sadora.Administracion {
    
    
    /// <summary>
    /// FrmValidarAccion
    /// </summary>
    public partial class FrmValidarAccion : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\..\Views\Administracion\FrmValidarAccion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.DialogHost MiniDialogo;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Views\Administracion\FrmValidarAccion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lAviso;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\Views\Administracion\FrmValidarAccion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnSi;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\Views\Administracion\FrmValidarAccion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnNo;
        
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
            System.Uri resourceLocater = new System.Uri("/Sadora;component/views/administracion/frmvalidaraccion.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Administracion\FrmValidarAccion.xaml"
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
            this.MiniDialogo = ((MaterialDesignThemes.Wpf.DialogHost)(target));
            return;
            case 2:
            this.lAviso = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.BtnSi = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\..\Views\Administracion\FrmValidarAccion.xaml"
            this.BtnSi.Click += new System.Windows.RoutedEventHandler(this.BtnSi_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.BtnNo = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\..\Views\Administracion\FrmValidarAccion.xaml"
            this.BtnNo.Click += new System.Windows.RoutedEventHandler(this.BtnNo_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

