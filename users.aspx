<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="envanterv2.users" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
   <script type="text/javascript">
		var selected_row_id = null;
		$(function () {
			var table = $("#udataGrid").jqGrid({
				url: 'data_channels/users_list.ashx',
				editurl: 'users.aspx',
				datatype: 'json',
				height: 350,
				colNames: ['Birim', 'Kullanıcı Adı', 'Şifre', 'E-Posta', 'Aktif?'],
				colModel: [
						{ name: 'division', index: 'division', width: 200, search: true, sortable: true, editable: true, stype: 'select', edittype: 'select', formatter: 'select',
							editoptions: { value: $.parseJSON(<%=get_ch%>), dataUrl: 'data_channels/children.ashx' }},
						{ name: 'UserName', width: 100, search: true, sortable: true, editable: true, editoptions: { size: 50 }},
						{ name: 'Password', width: 100, editable: true, editrules: { edithidden: true }, editoptions: { size: 50} },
						{ name: 'Email', width: 100, sortable: true, editable: true, editoptions: { size: 50} },
						{ name: 'IsLockedOut', width: 100, editable: true, stype:'select',  edittype: 'checkbox', formatter: "checkbox", editoptions: { size: 10, value: ":Hepsi;0:Yok;1:Var"} },

                    ],
				rowNum: 10,
				rowList: [10, 20, 30, 50],
				pager: '#uGridPager',
				rownumbers: true,
				sortname: 'UserName',
				viewrecords: true,
				sortorder: 'asc',
				caption: 'Tüm Kullanıcılar'
			});

			$("#udataGrid").jqGrid('navGrid', '#uGridPager',

			{ edit: true, add: true, del: true, search: false },
			// edit options
			{height: 270, width: 550, recreateForm: true, reloadAfterSubmit: true, closeAfterEdit: true, closeOnEscape: true, beforeShowForm: beforeEditFormShown },
			// add options
			{height: 270, width: 550, recreateForm: true, reloadAfterSubmit: true, closeAfterAdd: true, closeOnEscape: true, beforeShowForm: beforeAddFormShown },
			// del options
			{closeOnEscape: true, reloadAfterSubmit: true, width: 289, caption: "Kaydı Sil", msg: "Seçili kayıt(lar)ı silmek istediğinizden emin misiniz?", bSubmit: "Sil", bCancel: "Vazgeç" },
			// search options
			{}

			);
			$("#udataGrid").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
			
		});
		function beforeEditFormShown(form)
		{
//			$('#tr_amount', form).hide();

//			dlgDiv = $("#editmoddataGrid");
//			dlgDiv[0].style.top = Math.round((dlgDiv.parent().height()-dlgDiv.height())/2) + "px";
//			dlgDiv[0].style.left = Math.round((dlgDiv.parent().width()-dlgDiv.width())/2) + "px";

//			if( $("#editmoddataGrid select[id=owner] > option").length < 1 )
//			{
//				setTimeout(function() { beforeEditFormShown(form) },10);
//			}
//			else
//			{
//				$("#editmoddataGrid select[id=owner] > option").each(function (i, selected){
//					sel_id = $('#dataGrid').jqGrid('getGridParam', 'selrow');
//					owner = $('#dataGrid').jqGrid('getCell', sel_id, 'owner');

//					opt = selected.innerHTML;
//					opt.replace("&nbsp;", "");
//					owner.replace("&nbsp;", "");
//			
//					if( opt == owner )
//						selected.selected = true;
//				});
//			}
		} 
		function editFormFix(i, selected)
		{
//			sel_id = $('#dataGrid').jqGrid('getGridParam', 'selrow');
//			owner = $('#dataGrid').jqGrid('getCell', sel_id, 'owner');

//			opt = selected.innerHTML;
//			opt.replace("&nbsp;", "");
//			owner.replace("&nbsp;", "");
//			
//			if( opt == owner )
//				selected.selected = true;
		}
		function beforeAddFormShown(form)
		{
//			dlgDiv = $("#editmoddataGrid");
//			dlgDiv[0].style.top = Math.round((dlgDiv.parent().height()-dlgDiv.height())/2) + "px";
//			dlgDiv[0].style.left = Math.round((dlgDiv.parent().width()-dlgDiv.width())/2) + "px";

//			$('#tr_amount', form).show();
//			if( $("#editmoddataGrid select[id=owner] > option").length < 1 )
//			{
//				setTimeout(function() { beforeAddFormShown(form) },10);
//			}
//			else
//			{
//				sec_deg = $("#gview_dataGrid select[id=gs_owner]").find("option:selected").val();
//				$("#editmoddataGrid select[id=owner]").val(sec_deg);
//			}
		}

    </script>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<div id="mysearch"></div>
		<table id="udataGrid" cellpadding="0" cellspacing="0"></table>
		<div id="uGridPager"></div>
	</div>
	</form>
</body>
</html>
