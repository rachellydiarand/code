/*
Rachel Lydia Rand
javascript music player
written in 2025
*/


var songs = [];
var searchResults = [];
var playlist = [];
var playlistIndex = -1;
var loopFlip = -1;
var executionLoopInterval = -1;

var setListCurrentIndex = 0;

var setList = [[ 
	'C:/_rachel/mp3/daLibrary/audio/libraryRand/Billy Joel/03 Greatest Hits/2-08 Allentown.mp3',
	'C:/_rachel/mp3/daLibrary/audio/libraryRand/Rush/10 Grace Under Pressure/08 Between The Wheels.mp3',
	'C:/_rachel/mp3/daLibrary/audio/libraryRand/Billy Joel/01 The Stranger/07 She\'s Always A Woman.mp3',
	'C:/_rachel/mp3/daLibrary/audio/libraryRand/Rush/10 Grace Under Pressure/05 The Body Electric.mp3',
	'C:/_rachel/mp3/daLibrary/audio/libraryRand/Rush/10 Grace Under Pressure/02 Afterimage.mp3',
	'C:/_rachel/mp3/daLibrary/audio/libraryRand/Rush/10 Grace Under Pressure/04 The Enemy Within.mp3'], 
	[
	'http://legacy4.dottrombone.com/audio/libraryRand/_RandMix/Billy Joel/01 Allentown.mp3',
	'http://legacy4.dottrombone.com/audio/libraryRand/_RandMix/Billy Joel/01.5 Piano Man.mp3',
	'http://legacy4.dottrombone.com/audio/libraryRand/_RandMix/Billy Joel/02 The Stranger.mp3',
	'http://legacy4.dottrombone.com/audio/libraryRand/_RandMix/Billy Joel/03 Just The Way You Are.mp3',
	'http://legacy4.dottrombone.com/audio/libraryRand/_RandMix/Billy Joel/04 Scenes From An Italian Restaurant.mp3',
	'http://legacy4.dottrombone.com/audio/libraryRand/_RandMix/Billy Joel/05 Vienna.mp3',
	'http://legacy4.dottrombone.com/audio/libraryRand/_RandMix/Billy Joel/06 She\'s Always A Woman.mp3',
	'http://legacy4.dottrombone.com/audio/libraryRand/_RandMix/Billy Joel/07 Everybody Has A Dream.mp3']];

function mutateFile(file) {
	if(file.indexOf("http://legacy4.dottrombone.com") != -1) {
		//file = file.replace("http://legacy4.dottrombone.com", "http://192.168.143.43:4111");
		file = file.replace("http://legacy4.dottrombone.com", "");
	}
	
	return file;
}

function stopPlayer() {
	var player = document.getElementById('mp3player');
	player.pause();
	$("#mp3div").hide();
}

function rebuildPlaylist() {
	var str = "";
	for(var a = 0; a < playlist.length; a++) {
		str += '<a href="' + playlist[a] + '" target="_dtSong">' + playlist[a] + '</a> <span style="color:red;cursor: pointer;" onclick="javascript: removeSong(' + a + ');">X</span> <br />';
	}
	$("#playlist").html(str);
	//playlistIndex = -1;
}

function removeSong(i) {		
	var array = new Array();
	for(var z = 0; z < playlist.length; z++) {
		if(z != i) {
			//alert('remove');
			array.push(playlist[z]);
		}
		else {
			if(playlistIndex > z) {
				playlistIndex--;
			}
		}
	}
	
	if(playlistIndex != -1) {		
		if(playlistIndex < 0) playlistIndex = playlist.length - 1;
		if(playlistIndex > playlist.length - 1) playlistIndex = 0;
	}
	
	playlist = array;
	//alert(playlist.length);
	rebuildPlaylist();
}
	
function add(file) {
	file = mutateFile(file);
	playlist.push(file);
	rebuildPlaylist();
}

function addSetlist() {
	setListCurrentIndex++;
	if(setListCurrentIndex > setList.length -1) setListCurrentIndex = 0;
	for(var a = 0; a < setList[setListCurrentIndex].length; a++) {
		add(setList[setListCurrentIndex][a]);
	}
}

function addFromSearch() {
	if($("#addFromUrl").is(":checked")) {
		add($("#search").val());
		$("#search").val("");
	}
	else {
		for(var a = 0; a < searchResults.length; a++) {
			add(searchResults[a]);
		}
	}
}

function clearPlaylist() {
	playlist = [];
	$("#playlist").html("");
	//stopPlayer();
	playlistIndex = -1;
}	

function clearSearch() {
	$("#search").val("");
	search();
}

function search() {
	var term = $("#search").val().toLowerCase();
	var negativeTermArr = [];
	var subSet = [];
	
	var negativeTerm = "";
	if(term.indexOf("-") !== -1) {
		var nt2;
		negativeTerm = term.substr(term.indexOf("-") + 1);
		term = term.substr(0, term.indexOf(" -"));
		var negativeTerms = negativeTerm.split(",");
		for(var i = 0; i < negativeTerms.length; i++) {
			nt2 = negativeTerms[i].trim();
			console.log("nt2=" + nt2);
			if(nt2.length < 2) continue;
			negativeTermArr.push(nt2);
		}
	}
	else {
		term = term.trim();
	}		
	
	for(var a = 0; a < songs.length; a++) {
		if(songs[a].toLowerCase().indexOf(term) !== -1) {
			if(negativeTermArr.length > 0) {
				var hit = false;
				var nt;
				for(var i2 = 0; i2 < negativeTermArr.length; i2++) {
					nt = negativeTermArr[i2];
					console.log("nt="+nt);
					if(songs[a].toLowerCase().indexOf(nt) !== -1) {
						hit = true;
						break;
					}
				}
				
				if(!hit) subSet.push(songs[a]);
			}
			else {
				subSet.push(songs[a]);
			}
			
		}
	}
	
	var str = "";
	
	for(var a = 0; a < subSet.length; a++) {
		str += a + ") " + '<span class="song" file="' + subSet[a] + '">' + subSet[a] + '</span><br />';
	}
	
	$("#songs").html(str);
	
	$('.song').on("click", function () {
		var file = $(this).attr("file");
		add(file);
	});
	
	searchResults = subSet;
		
}

function play()
{
	playlistIndex = -1;
	playNextRadioFile();
}

function playLastRadioFile() {
	if(playlist.length == 0) return;
	if(loopFlip < 0) {
		playlistIndex--;
		if(playlistIndex < 0){
			playlistIndex = playlist.length - 1;
		}
		else {
			playlistIndex--;
		}
	}
	if(playlistIndex < 0) playlistIndex = playlist.length - 1;
	
	var file = playlist[playlistIndex];
	//file = "file:///" + file;
	//alert(file);
	
	$("#mp3src").attr("src", file);
	$("#mp3title").html(file);
	$("#mp3div").show();
	
	var player = document.getElementById('mp3player');
	player.load();
	player.play();
}

function playNextRadioFile() {
	if(playlist.length == 0) return;
	if(loopFlip < 0) playlistIndex++;
	if(playlistIndex > playlist.length - 1) playlistIndex = 0;
	
	var file = playlist[playlistIndex];
	//file = "file:///" + file;
	//alert(file);
	
	$("#mp3src").attr("src", file);
	$("#mp3title").html(file);
	$("#mp3div").show();
	
	var player = document.getElementById('mp3player');
	player.load();
	player.play();
}

function toggleLoop() {
	//alert("hit");
	loopFlip *= -1;
	if(loopFlip > 0) {
		$("#btnToggleLoop").html("<b>Loop On</b>");
	}
	else {
		$("#btnToggleLoop").html("Loop Off");
	}
}

function executionLoopIntervalEvent() {
	var player = document.getElementById('mp3player');
	if($("#mp3src").attr("src").indexOf("Midnight Tango") > -1) {
		//console.log("midnight tango position = " + player.currentTime);
		if(player.currentTime > 377) {
			// 6:17
			// clear interval so we don't enter a (potential) infinite callback loop
			clearInterval(executionLoopInterval);
			executionLoopInterval = setInterval(executionLoopIntervalEvent, 500);
			playNextRadioFile();
		}
	}
}

executionLoopInterval = setInterval(executionLoopIntervalEvent, 500);