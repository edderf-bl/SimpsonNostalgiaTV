"use strict";

const player = videojs('video', { autoplay: true });
const btnFullscreen = document.getElementById("btnFullscreen");
const btnVolumeDown = document.getElementById("btnVolumeDown");
const btnVolumeUp = document.getElementById("btnVolumeUp");

var connection = new signalR.HubConnectionBuilder().withUrl("/episode").build();

let episodeData;
let syncInterval;

connection.on("ReceiveEpisode", function (episode) {
	episodeData = JSON.parse(episode);
	episodeData.DatePlay = new Date(episodeData.DatePlay);

	var myplaylist = [
		{
			sources: [{
				src: episodeData.Path,
				type: 'video/mp4'
			}],
			title: episodeData.Title,
		}
	];

	player.playlist(myplaylist);

	try {
		player.play();
	} catch (error) {
		console.error(error);
	}
});

connection.start().then(function () {
	GetEpisode();
}).catch(function (err) {
	console.error(err)
});

function GetEpisode() {
	connection.invoke("GetRandomEpisode").catch(function (err) {
		return console.error(err.toString());
	});
};

function seekToCurrentPosition() {
	const currentTimeUTC = new Date();

	const secondsElapsed = (currentTimeUTC - episodeData.DatePlay) / 1000;

	const videoDuration = player.duration();

	if (secondsElapsed < videoDuration)
		player.currentTime(secondsElapsed);
	else if (!isNaN(videoDuration)) {
		GetEpisode();
	}
}

function startVideoSync() {
	seekToCurrentPosition();

	syncInterval = setInterval(() => {
		const currentPlayerTime = player.currentTime();
		const expectedTime = (new Date() - episodeData.DatePlay) / 1000;
		
		if (Math.abs(currentPlayerTime - expectedTime) > 20) {
			seekToCurrentPosition();
		}
	}, 1000);
}

function stopVideoSync() {
	clearInterval(syncInterval);
}

player.on('pause', () => {
	stopVideoSync();
});

player.on('play', () => {
	startVideoSync();
});

player.on('ended', function () {
	GetEpisode();
});

btnFullscreen.onclick = function () {
	player.muted(false);
	player.requestFullscreen();
}

btnVolumeDown.onclick = function () {
	player.muted(false);

	const volumeNow = player.volume();
	player.volume(volumeNow - 0.05); // -5%
};

btnVolumeUp.onclick = function () {
	player.muted(false);

	const volumeNow = player.volume();
	player.volume(volumeNow + 0.05); // +5%
};
