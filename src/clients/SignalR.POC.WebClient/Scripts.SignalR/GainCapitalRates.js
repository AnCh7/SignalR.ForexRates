// Crockford's supplant method (poor man's templating)
if (!String.prototype.supplant) {
	String.prototype.supplant = function(o) {
		return this.replace(/{([^{}]*)}/g,
			function(a, b) {
				var r = o[b];
				return typeof r === 'string' || typeof r === 'number' ? r : a;
			}
		);
	};
}

// A simple background color flash effect that uses jQuery Color plugin
jQuery.fn.flash = function(color, duration) {
	var current = this.css('backgroundColor');
	this.animate({ backgroundColor: 'rgb(' + color + ')' }, duration)
		.animate({ backgroundColor: current }, duration);
};

$(function() {
	var con = $.connection.hub;

	// Use global variable
	con.url = window.SignalRUrl;

	var forexRatesHubProxy = con.createHubProxy('GainCapitalRatesHub');
	forexRatesHubProxy.state.customerId = $("#customerId").val();

	forexRatesHubProxy.on('UpdateMarketPrice', function(market) {
		var $row = $(rowTemplate.supplant(market));

		var value = $MarketTableBody.find('tr[data-pair=' + market.PairNameShort + '] td:eq(1)').text();
		var prevBid = parseFloat(value).toFixed(6);
		var currBid = market.Bid.toFixed(6);

		if (currBid < prevBid) {
			bg = '255,148,148';
		}
		if (currBid > prevBid) {
			bg = '154,240,117';
		}
		if (currBid == prevBid) {
			bg = '239,238,239';
		}

		$MarketTableBody.find('tr[data-pair=' + market.PairNameShort + ']')
			.replaceWith($row);

		$row.flash(bg, 250);
	});

	forexRatesHubProxy.on('MarketOpened', function() {
		$("#open").prop("disabled", true);
		$("#close").prop("disabled", false);
		scrollTicker();
	});

	forexRatesHubProxy.on('MarketClosed', function() {
		$("#open").prop("disabled", false);
		$("#close").prop("disabled", true);
		stopTicker();
	});

	forexRatesHubProxy.on('StartQuoting', function() {
		ticker.invoke('StartQuoting');
	});

	forexRatesHubProxy.on('NotifyUser', function(message) {
		$('#status').text(message).css('color', 'blue');
	});

	con.connectionSlow(function() {
		$('#status').text("Connection Slow").css('color', 'red');
	});

	con.reconnecting(function() {
		$('#status').text("Reconnecting").css('color', 'red');
	});

	con.reconnected(function() {
		$('#status').text("Reconnected. Waiting for quotes").css('color', 'green');
	});

	con.disconnected(function() {
		$('#status').text("Disconnected").css('color', 'red');
	});

	con.logging = true;

	var ticker = forexRatesHubProxy,
		$MarketTable = $('#MarketTable'),
		$MarketTableBody = $MarketTable.find('tbody'),
		rowTemplate = '<tr ' +
			'data-pair="{PairNameShort}">' +
			'<td>{PairName}</td>' +
			'<td>{Bid}</td>' +
			'<td>{Ask}</td>' +
			'<td>{SpreadBid}</td>' +
			'<td>{SpreadAsk}</td></tr>',
		$MarketTicker = $('#MarketTicker'),
		$MarketTickerUl = $MarketTicker.find('ul'),
		liTemplate = '<li ' +
			'data-pair="{PairNameShort}">' +
			'<span class="symbol">{PairName}</span> ' +
			'<span class="bid">{Bid}</span> ' +
			'<span class="ask">{Ask}</span></li>';

	function scrollTicker() {
		var w = $MarketTickerUl.width();
		$MarketTickerUl.css({ marginLeft: w });
		$MarketTickerUl.animate({ marginLeft: -w }, 100000, 'linear', scrollTicker);
	}

	function stopTicker() {
		$MarketTickerUl.stop();
	}

	// Start the connection
	con.start()
		.pipe(init)
		.done(function() {
			$("#open").click(function() {
				ticker.invoke('StartQuoting');
			});
			$("#close").click(function() {
				ticker.invoke('StopQuoting');
			});

			$('#status').text("Connected").css('color', 'green');
		});

	function init() {
		return ticker.invoke('GetFirstQuotes').done(function(markets) {
			$MarketTableBody.empty();
			$MarketTickerUl.empty();
			$.each(markets, function() {
				var market = this;
				$MarketTableBody.append(rowTemplate.supplant(market));
				$MarketTickerUl.append(liTemplate.supplant(market));
			});
		});
	}
});