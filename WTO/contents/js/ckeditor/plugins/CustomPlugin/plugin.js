CKEDITOR.plugins.add('CustomPlugin',
{
    init: function (editor) {
        var pluginName = 'CustomPlugin';
        editor.ui.addButton('customplugin',
            {
                label: 'Insert Fields',
                command: 'InsertFields',
                icon: CKEDITOR.plugins.getPath('CustomPlugin') + 'Insert-16.png'
            });
        var cmd = editor.addCommand('InsertFields', { exec: AddFields });
    }
});

//function showMyDialog(e) {
//    $('[id$=divAddFieldsOverlay]').show();
//    $('[id$=divAddFields]').show();
//    $('[id$=hdnTemplateFieldsFor]').val('txtMessage');
//}