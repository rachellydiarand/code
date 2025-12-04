// javascript reverse post order
// not dependent on jquery
// by rachel lydia rand

function getAllAttributes(element) {
	var str = "";
	var attributes = element.attributes;
	for (var i = 0; i < attributes.length; i++) {
		str += " " + attributes[i].name + '="' + attributes[i].value + '"';
	}
	return str;
}

function reversePostOrder(parentId, childClass, startSlug, endSlug) {
	var posts = document.getElementsByClassName(childClass);
	if(posts.length > 0){
		var str = "";
		for(var a = posts.length - 1; a > -1; a--){
			str += '<' + posts[a].nodeName + getAllAttributes(posts[a]) + '>' + posts[a].innerHTML + '</' + posts[a].nodeName + '>';
		}
	}
	document.getElementById(parentId).innerHTML = '\r\n\r\n' + startSlug + '\r\n\r\n' + str + '\r\n\r\n' + endSlug + '\r\n\r\n';
}