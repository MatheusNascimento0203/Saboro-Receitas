import $ from 'jquery';

$.ajaxSetup({ cache: false });

['put', 'delete'].forEach(function (type) {
    $[type] = function (url, data) {
        return $.ajax({
            url: url,
            type: type.toUpperCase(),
            data: data ? JSON.stringify(data) : undefined,
            contentType: 'application/json'
        });
    };
});

export default $;