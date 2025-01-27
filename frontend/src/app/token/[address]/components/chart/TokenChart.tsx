"use client"
import { memo, useEffect, useRef } from "react"
import styles from "./TokenChart.module.scss"

type TokenChartProps = {
	symbol: string
}

const TokenChart = ({symbol}: TokenChartProps) => {
	const container = useRef<HTMLDivElement>(null)

	useEffect(() => {
		if (!symbol)
			return

		const script = document.createElement("script")
		script.src = "https://s3.tradingview.com/external-embedding/embed-widget-advanced-chart.js"
		script.type = "text/javascript"
		script.async = true
		script.innerHTML = `
		{
			"autosize": true,
			"symbol": "PUMP:${symbol.toUpperCase()}USD",
			"interval": "1",
			"timezone": "Etc/UTC",
			"theme": "dark",
			"style": "1",
			"locale": "en",
			"hide_side_toolbar": false,
			"allow_symbol_change": true,
			"calendar": false,
			"support_host": "https://www.tradingview.com"
		}`
		container?.current?.appendChild(script)

		return () => { container.current?.removeChild(script) }
	}, [symbol])

  return (
	<>
		<div className={styles.wrapper}>
			<div className={styles.chart}>
				<div className="tradingview-widget-container" ref={container} style={{ height: "100%", width: "100%" }}>
					<div className="tradingview-widget-container__widget" style={{ height: "calc(100% - 32px)", width: "100%" }}></div>
				</div>
			</div>
		</div>
	</>
  )
}

export default memo(TokenChart)
