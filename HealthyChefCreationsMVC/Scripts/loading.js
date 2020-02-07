$(document).ready(function () {
    MakeUpdateProg(false);
    //preventResfreshOnEnter();

    //datepicker init
    DatePickersInit();
});

function preventResfreshOnEnter()
{
    $('input').keypress(function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
        }
    });
}

function MakeUpdateProg(showProgress) {
    if (showProgress) {
        $(".updateProgressContainer").show();
        $(".updateProgressDisplay").show();
    }
    else {
        $(".updateProgressContainer").hide();
        $(".updateProgressDisplay").hide();
    }
}

function ShowHideLoader(isShow)
{
    if (isShow) {
        $("#divLoading").show();
    }
    else {
        $("#divLoading").hide();
    }
}


function GetApiBaseURL()
{
    var _baseURL = 'http://localhost:55995/api/';
   
    return _baseURL;
}


function ToggleMenus(option, submenu, mainmenu) {
    var active = 'active';
    var activeopen = 'active open';
    if (option !== undefined && option !== '') {
        $('#' + option).addClass(activeopen);
    }
    if (submenu !== undefined && submenu !== '') {
        $('#' + submenu).addClass(activeopen);
    }
    if (mainmenu !== undefined && mainmenu !== '') {
        $('#' + mainmenu).addClass(activeopen);
    }
}

function DatePickersInit()
{
    $('.datepicker').datepicker({
        dateFormat: 'mm-dd-yy',
        changeMonth: true,
        changeYear: true,
        onClose: function (dateText, inst) {
            $(this).blur(); 
        }
    });

    $('.datepicker1').datepicker({
        dateFormat: 'mm/dd/yy',
        changeMonth: true,
        changeYear: true,
        onClose: function (dateText, inst) {
            $(this).blur(); 
        }
    });
}
