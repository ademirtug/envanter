<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="envanter.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>

	<link href="css/south-street/jquery-ui-1.8.24.custom.css" rel="stylesheet" type="text/css" />
	<link href="src/css/ui.jqgrid.css" rel="stylesheet" type="text/css" />
	<link href="css/jquery.metro.css" rel="stylesheet" type="text/css" />

	<script src="jquery.min.js" type="text/javascript"></script>
	<script src="js/jquery-ui-1.8.24.custom.min.js" type="text/javascript"></script>
	

	<script src="jquery.metro.js" type="text/javascript"></script>
	<script src="js/jquery.jqGrid.min.js" type="text/javascript"></script>
	<script src="js/i18n/grid.locale-tr.js" type="text/javascript"></script>
	<script src="src/jqDnR.js" type="text/javascript"></script>

	<script type="text/javascript">

		$(document).ready(function () {
			$.ajaxSetup({ cache:false });

			$("div.metro-pivot").metroPivot();

			$("#login-box").hide();
			$("#users_div").hide();
			<%=jscmd%>
		});

		function open_login() {
			$("#login-box").dialog({ height: 280, width: 280, modal: true, closeOnEscape: false,
				open: function(event, ui) { $(".ui-dialog-titlebar-close", ui.dialog).hide(); }
			});
			
			$("#log_sub").button().click(function () {
				$.post("xlogin.ashx", { "username": $("#username").val(), "password": $("#password").val() },
				function (data) {
					if (data.Success == "OK") {
						$("#login-box").dialog("close");
						eval(data.Command);

						pivot_goTo("PC");
				
						$('#pc_display_all').empty();
						$('#pc_display_all').load('pc_display_all.aspx');
					}
				}, "json");	
			});
		};

		function logout()
		{
			$("#username").val("");
			$("#password").val("");
			 
			$.post("xlogin.ashx", { "act": "logout" },
				function (data) {
					if (data.Success == "OK") 
					{		
						pivot_goTo("PC");
						$('#pc_display_all').empty();
						eval(data.Command);
					}
			}, "json");
		}
    	function open_users() {
			$("div.metro-pivot").data("controller").addNewHeader("Kullanıcılar");
    	}
		function pivot_previous() { $("div.metro-pivot").data("controller").goToPrevious(); }
		function pivot_next() { $("div.metro-pivot").data("controller").goToNext(); }
		function pivot_goTo(header) { $("div.metro-pivot").data("controller").goToItemByName(header); }
		function pivot_goToIndex(index) { $("div.metro-pivot").data("controller").goToItemByIndex(index); }
	</script>
</head>
<body accent='blue' theme='light'>
	<form id="default_form" runat="server">
	<div class='page'>
		<div id="topbnnr" class='top-banner'>
				<%=usr%>
			</div>
		<h1 class='accent-color'>Envanter</h1>
		<div class='apptitle'>
			T.C<span class='accent-color'> Çevre ve Şehircilik </span>Bakanlığı <span style='font-size: 30pt;'>
				<a href='javascript:pivot_previous();'>&lsaquo;</a> 
				<a href='javascript:pivot_next();'>&rsaquo;</a>
			</span>
		</div>
		<div id="idx" class='metro-pivot'>
			<div class='pivot-item'>
				<h3><span style="color:Maroon">PC</span></h3>
				<span id="pc_display_all"></span>
			</div>
			<div class='pivot-item'>
				<h3><span style="color:Orange">Ofis Aygıtları</span></h3>
				<span id="office_display_all"></span>
			</div>
			<div class='pivot-item'>
				<h3><span style="color:Green">Yazılım</span></h3>
				<span id="software_display_all"></span>
			</div>
		</div>
	</div>
	<div id="login-box" title="Kullanıcı Girişi" class="ui-dialog-content ui-widget-content"  >
		<label>
			<span>Kullanıcı</span><br />
			<input id="username" type="text" />
		</label>
		<br /><br />
		<label>
			<span>Şifre</span><br />
			<input id="password" type="password" />
		</label><br /><br />
		<button id="log_sub" class="submit button" type="button">Giriş</button>
	</div>
	<div id="users_box">
		<div id="mysearch"></div>
		<table id="udataGrid" cellpadding="0" cellspacing="0"></table>
		<div id="udataGridPager"></div>
	</div>
	<span id="log"></span>
	</form>
</body>
</html>
