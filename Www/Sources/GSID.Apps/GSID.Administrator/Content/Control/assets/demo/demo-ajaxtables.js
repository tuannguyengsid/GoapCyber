$(document).ready(function(){
	change_content=function(content,page){
       $.ajax({ 
            type: 'GET', 
            url:page, 
            data:'', 
            success: function (data) {
                var serverData = JSON.parse(data);
                $(content).html("");
                $.each(serverData,function(index,obj) {
                    platform=obj.Platform;
                    browser=obj.browser;
                    css_grade=obj.css_grade;
                    engine_version=obj.engine_version;
                    rendering_engine=obj.rendering_engine;
                    $(content).append("<tr><td>"+rendering_engine+"</td><td>"+browser+"</td><td>"+platform+"</td><td>"+engine_version+"</td><td>"+css_grade+"</td></tr>");
                });
            }
        });
    };
    $("#select-number").on('change',function(){
    	row_number=parseInt($(this).val());
        $("#ajax-table").html("");
    	//page="ajax-table-data.php?row_number="+row_number;
        for(i=0;i<row_number;i++){
            rendering_engine="Rendering engine"+i;
            browser="Browser"+i;
            platform="Platform"+i;
            engine_version="Engine version"+i;
            css_grade="Css grade"+i;
            $("#ajax-table").append("<tr><td>"+rendering_engine+"</td><td>"+browser+"</td><td>"+platform+"</td><td>"+engine_version+"</td><td>"+css_grade+"</td></tr>");
        }
    	//change_content("#ajax-table",page);
    })
})