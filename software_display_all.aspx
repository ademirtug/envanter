<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="software_display_all.aspx.cs" Inherits="envanter.software_display_all" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    	$(function () {
    		var table = $("#sdataGrid").jqGrid({
    			url: 'data_channels/sf_list.ashx',
    			editurl: 'software_display_all.aspx',
    			datatype: 'json',
    			height: 350,

    			colNames: ['Birim', 'Yapımcı', 'Ürün Adı', 'Sürüm', 'Lisans', 'Adet'],
    			colModel: [
						{ name: 'owner', index: 'owner', width: 200, sortable: true, editable: true, stype: 'select', edittype: 'select', formatter: 'select',
							editoptions: { value: $.parseJSON(<%=get_ch%>), dataUrl: 'data_channels/children.ashx' }},
                        { name: 'brand', width: 100, search: true, sortable: true, editable: true, editoptions: { size: 10 }, editrules: { required: true} },
						{ name: 'product_name', width: 100, sortable: true, editable: true, editoptions: { size: 10} },
						{ name: 'version', width: 100, sortable: true, editable: true, editoptions: { size: 10} },
						{ name: 'have_license', width: 70, sortable: true, editable: true, stype:'select', edittype: 'checkbox', formatter: "checkbox", editoptions: { size: 10, value: ":Hepsi;0:Yok;1:Var"} },
    					{ name: 'amount', width: 100, sortable: false, editable: true, editrules: { number: true }, editoptions: { size: 10} }
					],
    			rowNum: 10,
    			rowList: [10, 20, 30, 50],
    			pager: '#sdataGridPager',
    			rownumbers: true,
    			sortname: 'brand',
    			viewrecords: true,
    			sortorder: 'asc',
    			caption: 'Tüm Yazılımlar'
    		});

    		globaltable = table;

    		$("#sdataGrid").jqGrid('navGrid', '#sdataGridPager',
			{ edit: true, add: true, del: true, search: false },
    		// edit options
			{height: 290, width:500, reloadAfterSubmit: true, closeAfterEdit: true, closeOnEscape: true, beforeShowForm: beforeEditFormShown },
    		// add options
			{height: 290, width:500, reloadAfterSubmit: true, closeAfterAdd: true, closeOnEscape: true, beforeShowForm: beforeAddFormShown },
    		// del options
			{closeOnEscape: true, reloadAfterSubmit: true, width: 289, caption: "Kaydı Sil", msg: "Seçili kayıt(lar)ı silmek istediğinizden emin misiniz?", bSubmit: "Sil", bCancel: "Vazgeç" },
    		// search options
			{});
    		$("#sdataGrid").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    	});

    	function beforeEditFormShown(form) {
			dlgDiv = $("#editmodsdataGrid");
			dlgDiv[0].style.top = Math.round((dlgDiv.parent().height()-dlgDiv.height())/2) + "px";
			dlgDiv[0].style.left = Math.round((dlgDiv.parent().width()-dlgDiv.width())/2) + "px";

			if( $("#editmodsdataGrid select[id=owner] > option").length < 1 )
			{
				setTimeout(function() { beforeEditFormShown(form) },10);
			}
			else
			{
				$('#tr_amount', form).show();

				$("#editmodsdataGrid select[id=owner] > option").each(function (i, selected){
					sel_id = $('#sdataGrid').jqGrid('getGridParam', 'selrow');
					owner = $('#sdataGrid').jqGrid('getCell', sel_id, 'owner');

					opt = selected.innerHTML;
					opt.replace("&nbsp;", "");
					owner.replace("&nbsp;", "");
			
					if( opt == owner )
						selected.selected = true;
				});
			}
    	}
    	function editFormFix(i, selected) {
    		sel_id = $('#sdataGrid').jqGrid('getGridParam', 'selrow');
    		owner = $('#sdataGrid').jqGrid('getCell', sel_id, 'owner');

    		opt = selected.innerHTML;
    		opt.replace("&nbsp;", "");
    		owner.replace("&nbsp;", "");

    		if (opt == owner)
    			selected.selected = true;
    	}
    	function beforeAddFormShown(form) {
			dlgDiv = $("#editmodsdataGrid");
			dlgDiv[0].style.top = Math.round((dlgDiv.parent().height()-dlgDiv.height())/2) + "px";
			dlgDiv[0].style.left = Math.round((dlgDiv.parent().width()-dlgDiv.width())/2) + "px";

    		$('#tr_amount').show();
			if( $("#editmodsdataGrid select[id=owner] > option").length < 1 )
			{
				setTimeout(function() { beforeAddFormShown(form) },10);
			}
			else
			{
				sec_deg = $("#gview_sdataGrid select[id=gs_owner]").find("option:selected").val();
				$("#editmodsdataGrid select[id=owner]").val(sec_deg);
			}
    	}		
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	<div id="mysearch"></div>
    <table id="sdataGrid" cellpadding="0" cellspacing="0"></table>
    <div id="sdataGridPager"></div>    
    </div>
    </form>
</body>
</html>
