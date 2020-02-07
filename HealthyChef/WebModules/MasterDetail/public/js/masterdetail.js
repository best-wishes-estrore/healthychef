
/*** Trim text length for display in MiniSummaryDisplay ***/
function autoEllipseText(textElementId, text, prefixElementId) {
	var padding = 15;
	
	textElement = document.getElementById(textElementId);

	// Need to compute the amount of space used up by existing elements in the parent container
	// so that we know how much space is leftover for the text string passed in.
	var childNodes = textElement.parentNode.childNodes;
	var usedSpace = 0;
	for (var i = 0; i < childNodes.length; i++) {
		if (childNodes[i].offsetWidth != null) {
			usedSpace = usedSpace + childNodes[i].offsetWidth;
		}
	}
	
	// Calculate the available text string space we have to work with
	var availableSpace = textElement.parentNode.offsetWidth - usedSpace - padding;
	
	// Insert the full text, and the test to see if we're over the max length
	textElement.innerHTML = '<span id="' + textElementId + 'ellipsisSpan" style="white-space:nowrap;">' + text + '</span>';
	inSpan = document.getElementById(textElementId + 'ellipsisSpan');
	
	if (inSpan.offsetWidth > availableSpace) {
		inSpan.innerHTML = '';
		var j = 1;
		// Text is too long, so we'll add one character at a time.
		while ((inSpan.offsetWidth < availableSpace) && (j < text.length)) {
			inSpan.innerHTML = text.substr(0, j) + '...';
			j++;
		}

		textElement.innerHTML = '<span id="' + textElement.id + 'ellipsisSpan" >' + inSpan.innerHTML + '</span>';
	}

	return;
}
