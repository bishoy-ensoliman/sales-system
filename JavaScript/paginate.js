/*global console, document, $, jQuery */
(function ($) {
    'use strict';

    function log(d) {
        console.log(d);
    }

    //    $(document).ready(function () {

    $.fn.pagination = function (options) {
        //        alert('prout');
        var paginationContainer = this,
            itemsPerPage,
            itemsToPaginate,
            defaults,
            settings,
            i,
            numberOfPaginationLinks;

        log(paginationContainer);

        defaults = {
            itemsPerPage: 3
        };

        settings = {};

        $.extend(settings, defaults, options);

        itemsPerPage = settings.itemsPerPage;

        itemsToPaginate = $(settings.itemsToPaginate);
        numberOfPaginationLinks = Math.ceil((itemsToPaginate.length / itemsPerPage));
        log(numberOfPaginationLinks);

        // Création de list dans la div ciblée
        $('<ul></ul>').prependTo(paginationContainer);

        // Boucle qui créer des li
        for (i = 0; i < numberOfPaginationLinks; i += 1) {
            paginationContainer.find('ul').append('<li>' + (i + 1) + '</li>');
        }

        itemsToPaginate.filter(':gt(' + (itemsPerPage - 1) + ')').hide();

        paginationContainer.find('ul li').click(function () {

            var itemsToHide,
                linkNumber,
                itemsToShow,
                $this = $(this);

            $this.addClass(settings.activeClass);
            $this.siblings().removeClass(settings.activeClass);
            
            linkNumber = $this.text();

            itemsToHide = itemsToPaginate.filter(':lt(' + ((linkNumber - 1) * itemsPerPage) + ')');

            $.merge(itemsToHide, itemsToPaginate.filter(':gt(' + ((linkNumber * itemsPerPage) - 1) + ')'));

            itemsToHide.hide();

            itemsToShow = itemsToPaginate.not(itemsToHide);
            itemsToShow.show();

        });
    };
    //    });

}(jQuery));