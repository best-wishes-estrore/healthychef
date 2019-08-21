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


function GetApiBaseURL()
{
     //var _baseURL = 'http://192.168.1.151:8090/api/';
    var _baseURL = 'http://localhost:55996/api/';
    //var _baseURL = 'https://admin.healthychefcreations.com/HealthyCHefWebAPI/api/';
    //var _baseURL = 'http://devadmin.healthychefcreations.com/HealthyChefAPI/api/';
    
   
    return _baseURL;
}


function ToggleMenus(option, submenu, mainmenu) {
    var active = 'active';
    var activeopen = 'active open';
    if (option !== undefined && option !== '') {
        $('#ctl00_' + option).addClass(activeopen);
    }
    if (submenu !== undefined && submenu !== '') {
        $('#ctl00_' + submenu).addClass(activeopen);
    }
    if (mainmenu !== undefined && mainmenu !== '') {
        $('#ctl00_' + mainmenu).addClass(activeopen);
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

    $('.datepicker-fridays').datepicker({
        dateFormat: 'mm/dd/yy',
        changeMonth: true,
        changeYear: true,
        onClose: function (dateText, inst) {
            $(this).blur();
        },
        beforeShowDay: function (date) {
            return [date.getDay() == 5];
        }
    });
    $('.datepicker-thursdays').datepicker({
        dateFormat: 'mm/dd/yy',
        changeMonth: true,
        changeYear: true,
        onClose: function (dateText, inst) {
            $(this).blur();
        },
        beforeShowDay: function (date) {
            return [date.getDay() == 4];
        }
    });
}
