﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace AplikacjaSerwisowa.kwronski {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WebServiceSoap", Namespace="http://kwronski.hostingasp.pl/")]
    public partial class WebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SayHelloToLameOperationCompleted;
        
        private System.Threading.SendOrPostCallback lamaOperationCompleted;
        
        private System.Threading.SendOrPostCallback XMLZwrocListeKntKartyOperationCompleted;
        
        private System.Threading.SendOrPostCallback ZwrocListeKntAdresyOperationCompleted;
        
        private System.Threading.SendOrPostCallback ZwrocListeKntKartyOperationCompleted;
        
        private System.Threading.SendOrPostCallback ZwrocListeZlecenSerwisowychNaglowkiOperationCompleted;
        
        private System.Threading.SendOrPostCallback ZwrocListeZlecenSerwisowychCzynnosciOperationCompleted;
        
        private System.Threading.SendOrPostCallback ZwrocListeZlecenSerwisowychSkladnikiOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WebService() {
            this.Url = "http://91.196.9.105:8080/WebService.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SayHelloToLameCompletedEventHandler SayHelloToLameCompleted;
        
        /// <remarks/>
        public event lamaCompletedEventHandler lamaCompleted;
        
        /// <remarks/>
        public event XMLZwrocListeKntKartyCompletedEventHandler XMLZwrocListeKntKartyCompleted;
        
        /// <remarks/>
        public event ZwrocListeKntAdresyCompletedEventHandler ZwrocListeKntAdresyCompleted;
        
        /// <remarks/>
        public event ZwrocListeKntKartyCompletedEventHandler ZwrocListeKntKartyCompleted;
        
        /// <remarks/>
        public event ZwrocListeZlecenSerwisowychNaglowkiCompletedEventHandler ZwrocListeZlecenSerwisowychNaglowkiCompleted;
        
        /// <remarks/>
        public event ZwrocListeZlecenSerwisowychCzynnosciCompletedEventHandler ZwrocListeZlecenSerwisowychCzynnosciCompleted;
        
        /// <remarks/>
        public event ZwrocListeZlecenSerwisowychSkladnikiCompletedEventHandler ZwrocListeZlecenSerwisowychSkladnikiCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://kwronski.hostingasp.pl/SayHelloToLame", RequestNamespace="http://kwronski.hostingasp.pl/", ResponseNamespace="http://kwronski.hostingasp.pl/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SayHelloToLame(string test) {
            object[] results = this.Invoke("SayHelloToLame", new object[] {
                        test});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SayHelloToLameAsync(string test) {
            this.SayHelloToLameAsync(test, null);
        }
        
        /// <remarks/>
        public void SayHelloToLameAsync(string test, object userState) {
            if ((this.SayHelloToLameOperationCompleted == null)) {
                this.SayHelloToLameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSayHelloToLameOperationCompleted);
            }
            this.InvokeAsync("SayHelloToLame", new object[] {
                        test}, this.SayHelloToLameOperationCompleted, userState);
        }
        
        private void OnSayHelloToLameOperationCompleted(object arg) {
            if ((this.SayHelloToLameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SayHelloToLameCompleted(this, new SayHelloToLameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://kwronski.hostingasp.pl/lama", RequestNamespace="http://kwronski.hostingasp.pl/", ResponseNamespace="http://kwronski.hostingasp.pl/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string lama() {
            object[] results = this.Invoke("lama", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void lamaAsync() {
            this.lamaAsync(null);
        }
        
        /// <remarks/>
        public void lamaAsync(object userState) {
            if ((this.lamaOperationCompleted == null)) {
                this.lamaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnlamaOperationCompleted);
            }
            this.InvokeAsync("lama", new object[0], this.lamaOperationCompleted, userState);
        }
        
        private void OnlamaOperationCompleted(object arg) {
            if ((this.lamaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.lamaCompleted(this, new lamaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://kwronski.hostingasp.pl/XMLZwrocListeKntKarty", RequestNamespace="http://kwronski.hostingasp.pl/", ResponseNamespace="http://kwronski.hostingasp.pl/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string XMLZwrocListeKntKarty() {
            object[] results = this.Invoke("XMLZwrocListeKntKarty", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void XMLZwrocListeKntKartyAsync() {
            this.XMLZwrocListeKntKartyAsync(null);
        }
        
        /// <remarks/>
        public void XMLZwrocListeKntKartyAsync(object userState) {
            if ((this.XMLZwrocListeKntKartyOperationCompleted == null)) {
                this.XMLZwrocListeKntKartyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnXMLZwrocListeKntKartyOperationCompleted);
            }
            this.InvokeAsync("XMLZwrocListeKntKarty", new object[0], this.XMLZwrocListeKntKartyOperationCompleted, userState);
        }
        
        private void OnXMLZwrocListeKntKartyOperationCompleted(object arg) {
            if ((this.XMLZwrocListeKntKartyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.XMLZwrocListeKntKartyCompleted(this, new XMLZwrocListeKntKartyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://kwronski.hostingasp.pl/ZwrocListeKntAdresy", RequestNamespace="http://kwronski.hostingasp.pl/", ResponseNamespace="http://kwronski.hostingasp.pl/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ZwrocListeKntAdresy() {
            object[] results = this.Invoke("ZwrocListeKntAdresy", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ZwrocListeKntAdresyAsync() {
            this.ZwrocListeKntAdresyAsync(null);
        }
        
        /// <remarks/>
        public void ZwrocListeKntAdresyAsync(object userState) {
            if ((this.ZwrocListeKntAdresyOperationCompleted == null)) {
                this.ZwrocListeKntAdresyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnZwrocListeKntAdresyOperationCompleted);
            }
            this.InvokeAsync("ZwrocListeKntAdresy", new object[0], this.ZwrocListeKntAdresyOperationCompleted, userState);
        }
        
        private void OnZwrocListeKntAdresyOperationCompleted(object arg) {
            if ((this.ZwrocListeKntAdresyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZwrocListeKntAdresyCompleted(this, new ZwrocListeKntAdresyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://kwronski.hostingasp.pl/ZwrocListeKntKarty", RequestNamespace="http://kwronski.hostingasp.pl/", ResponseNamespace="http://kwronski.hostingasp.pl/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ZwrocListeKntKarty() {
            object[] results = this.Invoke("ZwrocListeKntKarty", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ZwrocListeKntKartyAsync() {
            this.ZwrocListeKntKartyAsync(null);
        }
        
        /// <remarks/>
        public void ZwrocListeKntKartyAsync(object userState) {
            if ((this.ZwrocListeKntKartyOperationCompleted == null)) {
                this.ZwrocListeKntKartyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnZwrocListeKntKartyOperationCompleted);
            }
            this.InvokeAsync("ZwrocListeKntKarty", new object[0], this.ZwrocListeKntKartyOperationCompleted, userState);
        }
        
        private void OnZwrocListeKntKartyOperationCompleted(object arg) {
            if ((this.ZwrocListeKntKartyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZwrocListeKntKartyCompleted(this, new ZwrocListeKntKartyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://kwronski.hostingasp.pl/ZwrocListeZlecenSerwisowychNaglowki", RequestNamespace="http://kwronski.hostingasp.pl/", ResponseNamespace="http://kwronski.hostingasp.pl/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ZwrocListeZlecenSerwisowychNaglowki() {
            object[] results = this.Invoke("ZwrocListeZlecenSerwisowychNaglowki", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ZwrocListeZlecenSerwisowychNaglowkiAsync() {
            this.ZwrocListeZlecenSerwisowychNaglowkiAsync(null);
        }
        
        /// <remarks/>
        public void ZwrocListeZlecenSerwisowychNaglowkiAsync(object userState) {
            if ((this.ZwrocListeZlecenSerwisowychNaglowkiOperationCompleted == null)) {
                this.ZwrocListeZlecenSerwisowychNaglowkiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnZwrocListeZlecenSerwisowychNaglowkiOperationCompleted);
            }
            this.InvokeAsync("ZwrocListeZlecenSerwisowychNaglowki", new object[0], this.ZwrocListeZlecenSerwisowychNaglowkiOperationCompleted, userState);
        }
        
        private void OnZwrocListeZlecenSerwisowychNaglowkiOperationCompleted(object arg) {
            if ((this.ZwrocListeZlecenSerwisowychNaglowkiCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZwrocListeZlecenSerwisowychNaglowkiCompleted(this, new ZwrocListeZlecenSerwisowychNaglowkiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://kwronski.hostingasp.pl/ZwrocListeZlecenSerwisowychCzynnosci", RequestNamespace="http://kwronski.hostingasp.pl/", ResponseNamespace="http://kwronski.hostingasp.pl/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ZwrocListeZlecenSerwisowychCzynnosci() {
            object[] results = this.Invoke("ZwrocListeZlecenSerwisowychCzynnosci", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ZwrocListeZlecenSerwisowychCzynnosciAsync() {
            this.ZwrocListeZlecenSerwisowychCzynnosciAsync(null);
        }
        
        /// <remarks/>
        public void ZwrocListeZlecenSerwisowychCzynnosciAsync(object userState) {
            if ((this.ZwrocListeZlecenSerwisowychCzynnosciOperationCompleted == null)) {
                this.ZwrocListeZlecenSerwisowychCzynnosciOperationCompleted = new System.Threading.SendOrPostCallback(this.OnZwrocListeZlecenSerwisowychCzynnosciOperationCompleted);
            }
            this.InvokeAsync("ZwrocListeZlecenSerwisowychCzynnosci", new object[0], this.ZwrocListeZlecenSerwisowychCzynnosciOperationCompleted, userState);
        }
        
        private void OnZwrocListeZlecenSerwisowychCzynnosciOperationCompleted(object arg) {
            if ((this.ZwrocListeZlecenSerwisowychCzynnosciCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZwrocListeZlecenSerwisowychCzynnosciCompleted(this, new ZwrocListeZlecenSerwisowychCzynnosciCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://kwronski.hostingasp.pl/ZwrocListeZlecenSerwisowychSkladniki", RequestNamespace="http://kwronski.hostingasp.pl/", ResponseNamespace="http://kwronski.hostingasp.pl/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ZwrocListeZlecenSerwisowychSkladniki() {
            object[] results = this.Invoke("ZwrocListeZlecenSerwisowychSkladniki", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ZwrocListeZlecenSerwisowychSkladnikiAsync() {
            this.ZwrocListeZlecenSerwisowychSkladnikiAsync(null);
        }
        
        /// <remarks/>
        public void ZwrocListeZlecenSerwisowychSkladnikiAsync(object userState) {
            if ((this.ZwrocListeZlecenSerwisowychSkladnikiOperationCompleted == null)) {
                this.ZwrocListeZlecenSerwisowychSkladnikiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnZwrocListeZlecenSerwisowychSkladnikiOperationCompleted);
            }
            this.InvokeAsync("ZwrocListeZlecenSerwisowychSkladniki", new object[0], this.ZwrocListeZlecenSerwisowychSkladnikiOperationCompleted, userState);
        }
        
        private void OnZwrocListeZlecenSerwisowychSkladnikiOperationCompleted(object arg) {
            if ((this.ZwrocListeZlecenSerwisowychSkladnikiCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZwrocListeZlecenSerwisowychSkladnikiCompleted(this, new ZwrocListeZlecenSerwisowychSkladnikiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void SayHelloToLameCompletedEventHandler(object sender, SayHelloToLameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SayHelloToLameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SayHelloToLameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void lamaCompletedEventHandler(object sender, lamaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class lamaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal lamaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void XMLZwrocListeKntKartyCompletedEventHandler(object sender, XMLZwrocListeKntKartyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XMLZwrocListeKntKartyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal XMLZwrocListeKntKartyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void ZwrocListeKntAdresyCompletedEventHandler(object sender, ZwrocListeKntAdresyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZwrocListeKntAdresyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZwrocListeKntAdresyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void ZwrocListeKntKartyCompletedEventHandler(object sender, ZwrocListeKntKartyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZwrocListeKntKartyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZwrocListeKntKartyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void ZwrocListeZlecenSerwisowychNaglowkiCompletedEventHandler(object sender, ZwrocListeZlecenSerwisowychNaglowkiCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZwrocListeZlecenSerwisowychNaglowkiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZwrocListeZlecenSerwisowychNaglowkiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void ZwrocListeZlecenSerwisowychCzynnosciCompletedEventHandler(object sender, ZwrocListeZlecenSerwisowychCzynnosciCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZwrocListeZlecenSerwisowychCzynnosciCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZwrocListeZlecenSerwisowychCzynnosciCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void ZwrocListeZlecenSerwisowychSkladnikiCompletedEventHandler(object sender, ZwrocListeZlecenSerwisowychSkladnikiCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZwrocListeZlecenSerwisowychSkladnikiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZwrocListeZlecenSerwisowychSkladnikiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591