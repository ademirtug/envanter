<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="office_display_all.aspx.cs" Inherits="envanter.office_display_all" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    	$(function () {

    		var table = $("#odataGrid").jqGrid({
    			url: 'data_channels/office_list.ashx',
    			editurl: 'office_display_all.aspx',
    			datatype: 'json',
    			height: 350,
    			colNames: ['Birim', 'Marka', 'Model', 'Yazıcı', 'Çizici', 'Tarayıcı', 'Fax', 'Renkli', 'Alım Yılı', 'Adet'],
    			colModel: [
						{ name: 'owner', index: 'owner', width: 200, search: true, sortable: true, editable: true, stype: 'select', edittype: 'select', formatter: 'select',
							editoptions: { value: $.parseJSON(<%=get_ch%>), dataUrl: 'data_channels/children.ashx' }},
                        { name: 'brand', width: 100, search: true, sortable: true, editable: true, editoptions: { size: 10 }, editrules: { required: true} },
						{ name: 'model', width: 100, sortable: true, editable: true, editoptions: { size: 10} },
						{ name: 'have_printer', width: 100, sortable: true, stype:'select',  edittype: 'checkbox', formatter: "checkbox", editable: true, editoptions: { size: 10, value: ":Hepsi;0:Yok;1:Var"} },
						{ name: 'have_plotter', width: 70, sortable: true, stype:'select',edittype: 'checkbox', formatter: "checkbox", editable: true, editoptions: { size: 10, value: ":Hepsi;0:Yok;1:Var"} },
						{ name: 'have_scanner', width: 70, sortable: true, stype:'select',edittype: 'checkbox', formatter: "checkbox", editable: true, editoptions: { size: 10, value: ":Hepsi;0:Yok;1:Var"} },
						{ name: 'have_fax', width: 50,  sortable: true, stype:'select',edittype: 'checkbox', formatter: "checkbox", editable: true, editoptions: { size: 10, value: ":Hepsi;0:Yok;1:Var"} },
						{ name: 'color_print', width: 100, sortable: true, stype:'select',edittype: 'checkbox', formatter: "checkbox", editable: true, editoptions: { size: 10, value: ":Hepsi;0:Yok;1:Var"} },
						{ name: 'bought_at', width: 100, sortable: true, editable: true, editrules: { required: true, number: true, minValue: "1980", maxValue: "2025" }
						, editoptions: { size: 10 }
						},
    					{ name: 'amount', width: 100, search: false, sortable: false, hidden: true, editable: true, editrules: { edithidden: true }, editoptions: { size: 10} }
                    ],
    			rowNum: 10,
    			rowList: [10, 20, 30, 50],
    			pager: '#odataGridPager',
    			rownumbers: true,
    			sortname: 'brand',
    			viewrecords: true,
    			sortorder: 'asc',
    			caption: 'Tüm Ofis Aygıtları'
    		});

    		globaltable = table;

    		$("#odataGrid").jqGrid('navGrid', '#odataGridPager',
			{ edit: true, add: true, del: true, search: false },
    		// edit options
			{height: 370, width: 500, recreateForm: true, reloadAfterSubmit: true, closeAfterEdit: true, closeOnEscape: true, beforeShowForm: beforeEditFormShown },
    		// add options
			{height: 370, width: 550, recreateForm: true, reloadAfterSubmit: true, closeAfterAdd: true, closeOnEscape: true, beforeShowForm: beforeAddFormShown },
    		// del options
			{reloadAfterSubmit: true, width: 289, caption: "Kaydı Sil", msg: "Seçili kayıt(lar)ı silmek istediğinizden emin misiniz?", bSubmit: "Sil", bCancel: "Vazgeç" },
    		// search options
			{}
 			);

    		$("#odataGrid").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    	}); 
		function beforeEditFormShown(form)
		{
			dlgDiv = $("#editmododataGrid");
			dlgDiv[0].style.top = Math.round((dlgDiv.parent().height()-dlgDiv.height())/2) + "px";
			dlgDiv[0].style.left = Math.round((dlgDiv.parent().width()-dlgDiv.width())/2) + "px";

			if( $("#editmododataGrid select[id=owner] > option").length < 1 )
			{
				setTimeout(function() { beforeEditFormShown(form) },10);
			}
			else
			{
				$('#tr_amount', form).hide();

				$("#editmododataGrid select[id=owner] > option").each(function (i, selected){
					sel_id = $('#odataGrid').jqGrid('getGridParam', 'selrow');
					owner = $('#odataGrid').jqGrid('getCell', sel_id, 'owner');

					opt = selected.innerHTML;
					opt.replace("&nbsp;", "");
					owner.replace("&nbsp;", "");
			
					if( opt == owner )
						selected.selected = true;
				});
			}
		} 
		function editFormFix(i, selected)
		{
			sel_id = $('#odataGrid').jqGrid('getGridParam', 'selrow');
			owner = $('#odataGrid').jqGrid('getCell', sel_id, 'owner');

			opt = selected.innerHTML;
			opt.replace("&nbsp;", "");
			owner.replace("&nbsp;", "");
			
			if( opt == owner )
				selected.selected = true;
		}
		function beforeAddFormShown(form)
		{
			dlgDiv = $("#editmododataGrid");
			dlgDiv[0].style.top = Math.round((dlgDiv.parent().height()-dlgDiv.height())/2) + "px";
			dlgDiv[0].style.left = Math.round((dlgDiv.parent().width()-dlgDiv.width())/2) + "px";

			$('#tr_amount').show();

			if( $("#editmododataGrid select[id=owner] > option").length < 1 )
			{
				setTimeout(function() { beforeAddFormShown(form) },10);
			}
			else
			{
				sec_deg = $("#gview_odataGrid select[id=gs_owner]").find("option:selected").val();
				$("#editmododataGrid select[id=owner]").val(sec_deg);
			}
		}		
		       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<div id="mysearch"></div>
		<table id="odataGrid" cellpadding="0" cellspacing="0"></table>
		<div id="odataGridPager"></div>    
    </div>
    </form>
</body>
</html>
