CKEDITOR.editorConfig = function (config) {

    config.toolbar = 'Custom';
    //config.enterMode = CKEDITOR.ENTER_BR;
    config.forcePasteAsPlainText = true;
    config.toolbar_Custom =
[
//	['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', '-', 'About', 'Styles', 'TextColor', '-', 'Source']
	['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', 'Indent', 'Outdent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', "-", 'Link', 'Unlink', '-', 'Source', 'Styles', 'Format', 'TextColor', '-', 'Image']

];
    //config.colorButton_colors = '00923E,F8C100,28166F';
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    //config.uiColor = '#AADC6E';
    //    config.stylesSet = [{ name : 'Green Text', element : 'strong' },
    //                        { name : 'BAMFutura Orange', element : 'em' } ];
};
CKEDITOR.stylesSet.add('default',
[
// Inline Styles
	{ name: 'Blue Text', element: 'span', styles: { 'color': '#162F75' } },
	{ name: 'Purple Text', element: 'span', styles: { 'color': '#951A8D' } },
    { name: 'Grey Text', element: 'span', styles: { 'color': '#77787B;' } },
    { name: 'Red Text', element: 'span', styles: { 'color': '#EF3E33; font-size:1.3em;' } },
]);
//var editor = CKEDITOR.replace('txtMainTitle');
//CKFinder.setupCKEditor(null, '/admin/ckeditor/ckfinder/');


