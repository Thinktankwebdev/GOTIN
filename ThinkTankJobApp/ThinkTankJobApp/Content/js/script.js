/* Accordian */
$(document).ready(function() { 
	$('.accordion').each(function () {
		var $accordian = $(this);
		$accordian.find('.accordion-head').on('click', function () {
			$accordian.find('.accordion-head').removeClass('open');
			$(this).removeClass('open').addClass('closeAcc');
			$accordian.find('.accordion-body').slideUp();
			if (!$(this).next().is(':visible')) {
				$(this).removeClass('closeAcc').addClass('open');
				$(this).next().slideDown();
			}
		});
	});
	
	
});

/* Rating */
$(document).ready(function(){
	$(".rating input:radio").attr("checked", false);
	$('.rating input').click(function () { 
		$(".rating span").removeClass('checked');
		$(this).parent().addClass('checked');
	});

	$('input:radio').change(
	function(){
		var userRating = this.value;
	}); 
});

/* Delete notification */
$('.deleteNotify').on('click', function(){ 
  $(this).parent().remove();
});

$(document).ready(function(){
    $(".notifyBtn").click(function(){
        $("#notifyPopup").toggle();
    });
});

/* Add settings */
var serviceCount = 0;
function addService(section){
	
	var count = $("#" + section + " li").length;
	if (count == 10)  {
          alert("You have reached the limit of adding " + count + " inputs");
     }
	 else {
          var li = document.createElement("li");
          li.innerHTML = "<input type='text' class='form-control' name='service" + serviceCount + "'/> <div class='deleteNotify'><i class='fa fa-times-circle' aria-hidden='true'></i></div>";
          document.getElementById(section).appendChild(li);
          serviceCount++;
     }
}

/* Delete settings */
$(document).delegate('.deleteNotify', 'click', function()
{
	$(this).parent().remove();
});

/* Time picker dynamic */
$(document).delegate('.timepicker', 'click', function()
{
	$('.timepicker').timepicki();
});


/* Add timing */
var timingCount = 0;
function addTiming(section){
	
	var count = $("#" + section + " li").length; 
	if (count == 5)  {
          alert("You have reached the limit of adding " + count + " inputs");
     }
	 else {
          var li = document.createElement("li");
		  $(li).addClass("row");
		  
          li.innerHTML = "<div class='col-md-12'><select class='form-control' name='day" + timingCount + "' >												<option value='All'> All days </option> <option value='sun-thu'> Sunday - Thursday </option><option value='fri-sat'> Friday - Saturday </option><option value='sun'> Sunday </option><option value='mon'> Monday </option><option value='tue'> Tuesday </option><option value='wed'> Wednesday </option> <option value='thu'> Thursday </option><option value='fri'> Friday </option> <option value='Sat'> Saturday </option></select></div> ";
		  
		  li.innerHTML += "<div class='col-sm-6 timeSelect'><div class='xdisplay_inputx has-feedback indexpicker'><label>Start Time</label><input id='timepicker" + timingCount +"' class='form-control timepicker' type='text' name='timepicker" + timingCount + "' /></div></div>";
		  
		  li.innerHTML += "<div class='col-sm-6 timeSelect'><div class='xdisplay_inputx has-feedback indexpicker'><label>Start Time</label><input id='timepicker" + (timingCount + 1) +"' class='form-control timepicker' type='text' name='timepicker" + (timingCount + 1) + "' /></div></div><div class='deleteNotify'><i class='fa fa-times-circle' aria-hidden='true'></i></div>";
          
		  document.getElementById(section).appendChild(li);
          timingCount++;
     }
}

/* Edit settings */
function edit(section){ 
	var section = document.getElementById(section);
	var inputs = section.getElementsByTagName("input");
	for(var i = 0; i < inputs.length; i++) {
		inputs[i].disabled = false;
	}
	
	var selects = section.getElementsByTagName("select");
	for(var i = 0; i < selects.length; i++) {
		selects[i].disabled = false;
	}
	
	var textareas = section.getElementsByTagName("textarea");
	for(var i = 0; i < textareas.length; i++) {
		textareas[i].disabled = false;
	}
	
	var deleteIcon = section.getElementsByClassName("deleteNotify");
	for(var i = 0; i < deleteIcon.length; i++) {
		deleteIcon[i].style.display = "block";
	}
}

/* Tabs */
function openDtl(tab, listName) { 
	var i;
	var x = document.getElementsByClassName("tabDtl");
	for (i = 0; i < x.length; i++) {
	   x[i].style.display = "none";
	}
		
	tablinks = document.getElementsByClassName("tablinks");
	  for (i = 0; i < x.length; i++) {
		  tablinks[i].className = tablinks[i].className.replace(" selected", "");
	  }
	document.getElementById(listName).style.display = "block";
	tab.className += " selected";
}

/* Favorite button */
$(document).ready(function() {
    $('.favIcon').click(function() {
        if ($(this).hasClass("selected")) {
            $(this).removeClass('selected');
        } else {
            $(this).addClass('selected');
        }
    });
});

