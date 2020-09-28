$(function () {
        var austDay = new Date();
        austDay = new Date(austDay.getFullYear() + 1, 1 - 1, 26);
        $('#defaultCountdown').countdown({
                     until: austDay,
                     layout: ' {dn} {dl} {hn} {hl} {sn} {sl} '
       });
    $('#year').text(austDay.getFullYear());
});