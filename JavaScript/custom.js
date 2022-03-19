/*global console, document, $, jQuery */
(function ($) {
    'use strict';
    $(document).ready(function () {

        // Appel du plugin
        $('.pagination').pagination({
            itemsToPaginate: ".post",
            activeClass: 'active',
            itemsPerPage: 1
        });

    });

}(jQuery));