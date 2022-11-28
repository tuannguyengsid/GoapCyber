<?php
if (isset($_POST['html-content'])) {
    // sanitize & secure the posted HTML
    // https://www.bioinformatics.org/phplabware/internal_utilities/htmLawed/htmLawed_README.htm
    include_once 'htmLawed.php';
    $config = array(
        'comment'       => 1,
        'cdata'         => 1,
        'clean_ms_char' => 1,
        'keep_bad'      => 1,
        "safe"          => 1
    );
    $spec = '* = aria-*, data-*, role;';
    $processed = htmLawed($_POST['html-content'], $config, $spec);
}
?>
<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Bootstrap Plugin for TinyMCE - PHP Form Demo</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="author" content="Gilles Migliori">
    <meta name="copyright" content="miglisoft">
    <meta name="description" content="TinyMCE Textarea with Bootstrap 4 toolbar - post the form to load the textarea content on your page and test your content on the fly.">
    <link href="https://www.tinymce-bootstrap-plugin.com/demo/php-form-demo/index.php" rel="canonical">
    <meta name="language" content="en">
    <meta name="robots" content="index, follow">
    <!-- Bootstrap 4 CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
    <!-- TinyMCE -->
    <script src="https://cdn.jsdelivr.net/npm/tinymce@5.1.5/tinymce.min.js"></script>
    <!-- Bootstrap plugin -->
    <script src="../../tinymce-bootstrap-plugin/plugin.js"></script>
</head>

<body class="d-flex flex-column justify-content-between" style="min-height: 100vh">

    <?php if (!isset($_POST['html-content'])) { ?>

        <div class="container pt-4">
            <h1 class="text-secondary font-weight-light text-center mb-4">Bootstrap Plugin for TinyMCE<br><small>PHP Form Demo</small></h1>

            <p class="lead mb-2">Enter your content in the TinyMCE editor then validate to display the result.</p>

            <p class="lead">This page loads jQuery and Bootstrap Javascript. It allows you to test Javascript functionalities such as Modal, Collapse, Dropdown, Popover, etc...</p>

            <!--=====================================
            =                  Form                 =
            ======================================-->

            <form method="POST">
                <div class="form-group">
                    <textarea name="html-content" class="tinymce" style="min-height:50vh"></textarea>
                </div>
                <div class="form-group text-center mt-5">
                    <button type="submit" class="btn btn-lg btn-success">Submit <i class="fas fa-envelope ml-2"></i></button>
                </div>
            </form>
        </div>

        <!--=========  End of Form  =========-->

        <?php } elseif (isset($processed)) { ?>

        <!--=====================================
        =                Output                 =
        ======================================-->
            <h1 class="text-secondary font-weight-light text-center my-4">Bootstrap Plugin for TinyMCE<br><small>Posted Bootstrap 4 HTML</small></h1>
            <div id="output"><?php echo $processed; ?></div>

            <p class="text-center">
                <button type="button" onclick="window.location = window.location.href;" class="btn btn-lg btn-primary">Reload the editor <i class="fas fa-sync ml-2"></i></button>
            </p>

        <!--=======  End of Output  =========-->

        <?php } ?>

    <p class="text-center mt-4"><a href="https://www.tinymce-bootstrap-plugin.com/" title="TinyMCE Bootstrap plugin">&COPY; TinyMCE Bootstrap plugin</a></p>
    <script src="https://kit.fontawesome.com/dc840779d0.js" crossorigin="anonymous"></script>
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script>
        if (typeof (base_url) == "undefined") {
            var base_url = location.protocol + '//' + location.host + '/';

            const LOCAL_DOMAINS = ["localhost", "127.0.0.1", 'tinymce-bootstrap-plugin-scratch'];
            if (LOCAL_DOMAINS.includes(window.location.hostname)) {
                var tbpKey = 'chty0ssnHokCWxzrCoF41/nCKWDH3zF7cLo5Xb3jTABH4Mdok+seIAin4kOc3eAUfbLI0oMsREGyaseCZA1OIxbra2WnAFW9ycKpCktLR9ZoDDz78MMq08KJws/NdPhJ';
            } else {
                var tbpKey = 'production-key-here';
            }
        }
        // uncomment the following line to test if your key is properly set
        // console.log(tbpKey);
        tinymce.init({
            selector: 'textarea.tinymce',
            plugins: 'advlist autolink bootstrap link image lists charmap print preview help table',
            toolbar: [
            'undo redo | bootstrap',
            'cut copy paste | styleselect | alignleft aligncenter alignright alignjustify | bold italic | link image | preview | help'],
            contextmenu: "link image imagetools table spellchecker | bootstrap",
            file_picker_types: 'file image media',
            bootstrapConfig: {
                url: base_url + 'tinymce-bootstrap-plugin/',
                iconFont: 'fontawesome5',
                imagesPath: '/demo/demo-images',
                key: tbpKey
            },
            formats: {
            alignleft: {selector : 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table,img', classes : 'text-left'},
            aligncenter: {selector : 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table,img', classes : 'text-center'},
            alignright: {selector : 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table,img', classes : 'text-right'},
            alignjustify: {selector : 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table,img', classes : 'text-justify'},
            bold: {inline : 'strong'},
            italic: {inline : 'em'},
            underline: {inline : 'u'},
            sup: {inline : 'sup'},
            sub: {inline : 'sub'},
            strikethrough: {inline : 'del'}
            },
            style_formats_autohide: true
        });
    </script>
</body>

</html>
