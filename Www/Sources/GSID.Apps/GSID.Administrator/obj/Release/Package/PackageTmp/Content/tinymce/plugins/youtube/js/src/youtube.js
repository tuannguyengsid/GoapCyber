/*
 # -- BEGIN LICENSE BLOCK ----------------------------------
 #
 # This file is part of tinyMCE.
 # YouTube for tinyMCE 4.x.x
 # Copyright (C) 2011 - 2017  Gerits Aurelien <aurelien[at]magix-cms[dot]com>
 # This program is free software: you can redistribute it and/or modify
 # it under the terms of the GNU General Public License as published by
 # the Free Software Foundation, either version 3 of the License, or
 # (at your option) any later version.
 #
 # This program is distributed in the hope that it will be useful,
 # but WITHOUT ANY WARRANTY; without even the implied warranty of
 # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 # GNU General Public License for more details.

 # You should have received a copy of the GNU General Public License
 # along with this program.  If not, see <http://www.gnu.org/licenses/>.
 #
 # -- END LICENSE BLOCK -----------------------------------
 * https://developers.google.com/youtube/player_parameters
 */
(function ($) {
    var timer;

    /*
     * Return youtube id
     * @param url {string}
     * @return {string|boolean}
     */
    function youtubeId(url) {
        var match = url.match((/^.*(youtu\.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/));
        return match && match[2].length === 11 ? match[2] : false;
    }

    /**
     * Return YouTube convertUrl URL
     * @param url {string}
     * @param iframe {bool} embed or iframe
     * @returns {string}
     */
    function convertUrl(url) {
        var id = youtubeId(url);
        if (url && id) {
            //url = "https://www.youtube.com/" + (iframe ? "embed/" : "v/") + youtubeId(url);
            url = "https://www.youtube.com/" + "embed/" + youtubeId(url);
        }
        return url;
    }

    /**
     * Format HTML
     * @param iframe {boolean}
     * @param width {number}
     * @param height {number}
     * @param data {string}
     * @returns {string}
     */
    function dataToHtml(width, height, data) {
        var dim;
        if (data) {
            dim = 'width="' + width + '" height="' + height + '"';
            return '<iframe src="' + data + '" ' + dim + ' frameborder="0" allowfullscreen class="embed-responsive-item">&nbsp;</iframe>';
        }
    }

    /**
     * Insert content when the window form is submitted
     * @returns {string}
     */
    function insert() {
        var result,
            options = "",
            youtubeAutoplay = $("#youtubeAutoplay").is(":checked"),
            youtubeREL = $("#youtubeREL").is(":checked"),
            youtubeHD = $("#youtubeHD").is(":checked"),
            width = $("#youtubeWidth").val(),
            height = $("#youtubeHeight").val(),
            newYouTubeUrl = convertUrl($('#youtubeID').val());

        //SELECT Include related videos
        if (youtubeREL) {
            options += "?rel=1";
        }else{
            options += "?rel=0";
        }

        //SELECT Watch in HD
        if (youtubeHD) {
            options += "&hd=1";
        }else{
            options += "&hd=0";
        }
        // Youtube Autoplay
        if (youtubeAutoplay) {
            options += "&autoplay=1";
        }
        if (newYouTubeUrl) {
            // Insert the contents from the input into the document
            //result = dataToHtml(html5State, width, height, newYouTubeUrl + (html5State ? "" : options));
            result = dataToHtml(width, height, newYouTubeUrl + options);
        }
        return result;
    }

    function preview() {
        $("#preview").html(
            dataToHtml(750, 315, convertUrl($('#youtubeID').val()))
        );
    }

    /**
     * Update Timer with keypress
     * @param ts {number} (optional)
     */
    function updateTimer(ts) {
        clearTimeout(timer);
        timer = setTimeout(preview, ts || 1000);
    }

    /**
     * Execute insert
     */
    function run() {
        var data = insert();
        if (data) {
            parent.tinymce.activeEditor.insertContent(data);
        }
        parent.tinymce.activeEditor.windowManager.close();
    }

    /**
     * Execute preview
     */
    function runPreview() {
        if ($("#preview").length) {
            $('#youtubeID').keypress(function () {
                updateTimer();
            }).change(function () {
                updateTimer(100);
            });
        }
    }

    /**
     * Execute namespace youtube
     */
    $(function () {
        // Init templatewith mustach
        var data = {
            youtubeurl: parent.tinymce.util.I18n.translate("Youtube URL"),
            youtubeID: parent.tinymce.util.I18n.translate("Youtube ID"),
            youtubeWidth: parent.tinymce.util.I18n.translate("width"),
            youtubeHeight: parent.tinymce.util.I18n.translate("height"),
            youtubeAutoplay: parent.tinymce.util.I18n.translate("autoplay"),
            youtubeHD: parent.tinymce.util.I18n.translate("HD video"),
            youtubeREL: parent.tinymce.util.I18n.translate("Related video"),
            cancel: parent.tinymce.util.I18n.translate("cancel"),
            Insert: parent.tinymce.util.I18n.translate("Insert")
        };

        //Use jQuery's get method to retrieve the contents of our template file, then render the template.
        $.get("view/forms.html", function (template) {
            $("#template-container").append(Mustache.render(template, data));
            runPreview();
            $("#insert-btn").on("click", run);
            $("#close-btn").on("click", function(){
                parent.tinymce.activeEditor.windowManager.close();
            });
        });
    });
}(jQuery));