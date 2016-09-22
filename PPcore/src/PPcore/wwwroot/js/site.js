$.fn.clearValidation = function(){var v = $(this).validate();$('[name]',this).each(function(){v.successList.push(this);v.showErrors();});v.resetForm();v.reset();};

function setTableMembers(tableId) {
    var tableMember = tableId.DataTable({
        responsive: true,
        "ordering": false,
        "oLanguage": {
            "sLengthMenu": "แสดงผล _MENU_ รายการ/หน้า",
            "sZeroRecords": "ไม่เจอข้อมูลที่ค้นหา",
            "sInfo": "แสดงรายการ _START_ ถึง _END_ ของทั้งหมด _TOTAL_ รายการ",
            "sInfoEmpty": "ไม่พบรายการ",
            "sInfoFiltered": "(จากทั้งหมด _MAX_ รายการ)",
            "sSearch": "ค้นหา :",
            "oPaginate": {
                "sFirst": "หน้าแรก",
                "sLast": "หน้าสุดท้าย",
                "sNext": "ถัดไป",
                "sPrevious": "ก่อนหน้า"
            }
        }, bAutoWidth: false,
        "columnDefs": [
            { "targets": [0], "width": "15", "className": "dt-center" },
            { "targets": [1], "visible": false, "searchable": false },
            { "targets": [2], "width": "30", "className": "dt-center" },
            { "targets": [3], "width": "50", "className": "dt-center" },
            { "targets": [4], "width": "150" },
            { "targets": [5], "width": "70", "className": "dt-center" },
            { "targets": [6], "width": "15", "className": "dt-center" },
            { "targets": [7], "width": "50", "className": "dt-center" },

            { "targets": [9], "width": "50", "className": "dt-center" },
            { "targets": [10], "width": "50", "className": "dt-center" },
            { "targets": [11], "width": "50" },
            { "targets": [12], "width": "50" },
            { "targets": [13], "width": "50" },
        ], fixedColumns: true,
        preDrawCallback: function (settings) {
            var api = new $.fn.dataTable.Api(settings);
            var pagination = $(this)
                .closest('.dataTables_wrapper')
                .find('.dataTables_paginate');

            if (api.page.info().pages <= 1) {
                pagination.hide();
            }
            else {
                pagination.show();
            }
        }
    });

    tableId.find('tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            tableMember.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    return tableMember;
}
function setDataTablesSimple() {
    var tableMember = $('#dataTablesSimple').DataTable({
        responsive: true,
        "ordering": false,
        "oLanguage": {
            "sLengthMenu": "แสดงผล _MENU_ รายการ/หน้า",
            "sZeroRecords": "ไม่เจอข้อมูลที่ค้นหา",
            "sInfo": "แสดงรายการ _START_ ถึง _END_ ของทั้งหมด _TOTAL_ รายการ",
            "sInfoEmpty": "ไม่พบรายการ",
            "sInfoFiltered": "(จากทั้งหมด _MAX_ รายการ)",
            "sSearch": "ค้นหา :",
            "oPaginate": {
                "sFirst": "หน้าแรก",
                "sLast": "หน้าสุดท้าย",
                "sNext": "ถัดไป",
                "sPrevious": "ก่อนหน้า"
            }
        }, bAutoWidth: false,
        "columnDefs": [
            { "targets": [0], "width": "50", "className": "dt-center" },
            { "targets": [1], "visible": false, "searchable": false }
        ], fixedColumns: true,
        preDrawCallback: function (settings) {
            var api = new $.fn.dataTable.Api(settings);
            var pagination = $(this)
                .closest('.dataTables_wrapper')
                .find('.dataTables_paginate');

            if (api.page.info().pages <= 1) {
                pagination.hide();
            }
            else {
                pagination.show();
            }
            //$('.dataTables_filter input').addClass('form-control').css({"width":"15em"});
            //$('.dataTables_length select').addClass("form-control");
        }
    });

    $('#dataTablesSimple tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            tableMember.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    return tableMember;
}
function setDataTables(tableId) {
    var tableMember = tableId.DataTable({
        responsive: true,
        "ordering": false,
        "oLanguage": {
            "sLengthMenu": "แสดงผล _MENU_ รายการ/หน้า",
            "sZeroRecords": "ไม่เจอข้อมูลที่ค้นหา",
            "sInfo": "แสดงรายการ _START_ ถึง _END_ ของทั้งหมด _TOTAL_ รายการ",
            "sInfoEmpty": "ไม่พบรายการ",
            "sInfoFiltered": "(จากทั้งหมด _MAX_ รายการ)",
            "sSearch": "ค้นหา :",
            "oPaginate": {
                "sFirst": "หน้าแรก",
                "sLast": "หน้าสุดท้าย",
                "sNext": "ถัดไป",
                "sPrevious": "ก่อนหน้า"
            }
        },
        "columnDefs": [{
            "targets": [1],
            "visible": false,
            "searchable": false
        }],
        preDrawCallback: function (settings) {
            var api = new $.fn.dataTable.Api(settings);
            var pagination = $(this)
                .closest('.dataTables_wrapper')
                .find('.dataTables_paginate');

            if (api.page.info().pages <= 1) {
                pagination.hide();
            }
            else {
                pagination.show();
            }
            //$('.dataTables_filter input').addClass('form-control').css({"width":"15em"});
            //$('.dataTables_length select').addClass("form-control");
        }
    });
    tableId.find('tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            tableMember.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    return tableMember;
}

function setTableProductDetailsAsTableList(tableId) {
    var tableMember = tableId.DataTable({
        responsive: true,
        "ordering": false,
        "bLengthChange": false,
        "pageLength": 5,
        "oLanguage": {
            "sLengthMenu": "แสดงผล _MENU_ รายการ/หน้า",
            "sZeroRecords": "ไม่เจอข้อมูลที่ค้นหา",
            "sInfo": "แสดงรายการ _START_ ถึง _END_ ของทั้งหมด _TOTAL_ รายการ",
            "sInfoEmpty": "ไม่พบรายการ",
            "sInfoFiltered": "(จากทั้งหมด _MAX_ รายการ)",
            "sSearch": "ค้นหา :",
            "oPaginate": {
                "sFirst": "หน้าแรก",
                "sLast": "หน้าสุดท้าย",
                "sNext": "ถัดไป",
                "sPrevious": "ก่อนหน้า"
            }
        }, bAutoWidth: false,
        "columnDefs": [
            { "targets": [0], "width": "50", "className": "dt-center" },
            { "targets": [1], "width": "80", "className": "dt-center" },
            { "targets": [3], "width": "50", "className": "dt-center" }
        ],fixedColumns: true,
        preDrawCallback: function (settings) {
            var api = new $.fn.dataTable.Api(settings);
            var pagination = $(this)
                .closest('.dataTables_wrapper')
                .find('.dataTables_paginate');

            if (api.page.info().pages <= 1) {
                pagination.hide();
            }
            else {
                pagination.show();
            }
            //$('.dataTables_filter input').addClass('form-control').css({"width":"15em"});
            //$('.dataTables_length select').addClass("form-control");
        }
    });
    tableId.find('tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            tableMember.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    return tableMember;
}


//--------------------------------------------------------------------------------------------
function setTable_default(tableId, w) {
    var d = tableId.DataTable({
        responsive: true,
        "ordering": false,
        "oLanguage": {
            "sLengthMenu": "แสดงผล _MENU_ รายการ/หน้า",
            "sZeroRecords": "ไม่เจอข้อมูลที่ค้นหา",
            "sInfo": "แสดงรายการ _START_ ถึง _END_ ของทั้งหมด _TOTAL_ รายการ",
            "sInfoEmpty": "ไม่พบรายการ",
            "sInfoFiltered": "(จากทั้งหมด _MAX_ รายการ)",
            "sSearch": "ค้นหา :",
            "oPaginate": {
                "sFirst": "หน้าแรก",
                "sLast": "หน้าสุดท้าย",
                "sNext": "ถัดไป",
                "sPrevious": "ก่อนหน้า"
            }
        }, bAutoWidth: false,
        "columnDefs": w,
        fixedColumns: true, 
        preDrawCallback: function (settings) {
            var api = new $.fn.dataTable.Api(settings);
            var pagination = $(this)
                .closest('.dataTables_wrapper')
                .find('.dataTables_paginate');

            if (api.page.info().pages <= 1) {
                pagination.hide();
            }
            else {
                pagination.show();
            }
        }
    });

    tableId.find('tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            d.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    return d;
}

function setTable_PageN(tableId, w, n) {
    var d = tableId.DataTable({
        responsive: true,
        "ordering": false,
        "bLengthChange": false,
        "pageLength": n,
        "oLanguage": {
            "sLengthMenu": "แสดงผล _MENU_ รายการ/หน้า",
            "sZeroRecords": "ไม่เจอข้อมูลที่ค้นหา",
            "sInfo": "แสดงรายการ _START_ ถึง _END_ ของทั้งหมด _TOTAL_ รายการ",
            "sInfoEmpty": "ไม่พบรายการ",
            "sInfoFiltered": "(จากทั้งหมด _MAX_ รายการ)",
            "sSearch": "ค้นหา :",
            "oPaginate": {
                "sFirst": "หน้าแรก",
                "sLast": "หน้าสุดท้าย",
                "sNext": "ถัดไป",
                "sPrevious": "ก่อนหน้า"
            }
        }, bAutoWidth: false,
        "columnDefs": w,
        fixedColumns: true,
        preDrawCallback: function (settings) {
            var api = new $.fn.dataTable.Api(settings);
            var pagination = $(this)
                .closest('.dataTables_wrapper')
                .find('.dataTables_paginate');

            if (api.page.info().pages <= 1) {
                pagination.hide();
            }
            else {
                pagination.show();
            }
        }
    });

    tableId.find('tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            d.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    return d;
}

function setTableMinimal(tableId, w) {
    var d = tableId.DataTable({
        responsive: false,
        "paging": false,
        "searching": false,
        "info": false,
        "ordering": false,
        "sZeroRecords": "ไม่เจอข้อมูลที่ค้นหา",
        "sInfoEmpty": "ไม่พบรายการ",
        bAutoWidth: false,
        "columnDefs": w,
        fixedColumns: true,
        "autoWidth": false
    });

    tableId.find('tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            d.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    return d;
}
