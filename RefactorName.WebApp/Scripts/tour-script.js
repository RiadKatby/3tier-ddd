(function(){
	
	var tour = new Tour({
		storage : false,
				 backdrop: true,
  backdropContainer: 'body',
  backdropPadding: true,
	});

	tour.addSteps([
	{
		element: ".tour-step.tour-step-one",
		placement: "bottom",
		title: "Welcome to our landing page!",
		content: "What's your name? <br><input class='form-control' type='text' name='your_name'>",
				

	},
	{
		element: ".tour-step.tour-step-two",
		placement: "top",
		title: "second title",
		content: "Here are the sections of this page, easily laid out.",

	},
	{
		element: ".tour-step.tour-step-three",
		placement: "left",
		title: "Main section",
		content: "This is a section that you can read. It has valuable information."
	},
	{
		element: ".tour-step.tour-step-four",
		placement: "right",
		title: "Thank you.",
		content: "done"
	},

	]);

	// Initialize the tour
	tour.init();

	// Start the tour
	
	
	$('.tour-highlight').click(function(){
		tour.start();
		tour.restart();
		// $("body").append("<div id='overlay' style='background:#767475;position:fixed;top:0;bottom:0;left:0;right:0;z-index:99;'></div>");
		// $('.tour-step').addClass('tour-step-show');
		// $('#overlay').click(function(){
		// 	$('#overlay').fadeOut(300);
		// 	$('#overlay').remove();
		// 	$(".tour-popover").remove();
		// });
		// localStorage.clear();
		
	});

	// $(document).on('click','.close-btn',function (e) {
	// 	$('#overlay').fadeOut(300);
	// 	$('#overlay').remove();
	// 	$(".tour-popover").remove();
	// 	localStorage.clear();
	// });


}());


