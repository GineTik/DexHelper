"use client"
import { useEffect, useRef, useState } from "react"
import styles from "./TokenBlocks.module.scss"
import BuysSellsBlock from "./components/blocks/buys-sells/BuysSellsBlock"
import FiltrationReportBlock from "./components/blocks/filtration-report/FiltrationReportBlock"
import HoldersBlock from "./components/blocks/holders/HoldersBlock"
import InformationBlock from "./components/blocks/information/InformationBlock"
import TransactionsBlock from "./components/blocks/transactions/TransactionsBlock"
import BlockTabs from "./components/tabs/BlocksTabs"

const TokenBlocks = () => {
	const wrapperRef = useRef<HTMLDivElement>(null)
	const blocksRef = useRef<HTMLDivElement>(null)

	const [isBlocksBeyond, setIsBlocksBeyond] = useState(false)

	useEffect(() => {
		const updateWindowDimensions = () => {
			if (!wrapperRef.current || !blocksRef.current)
				return

			const wrapperWidth = wrapperRef.current?.getBoundingClientRect().width ?? 0
			const blocksWidth = blocksRef.current?.getBoundingClientRect().width ?? 0
			const isBeyond = blocksWidth > wrapperWidth

			setIsBlocksBeyond(isBeyond)
		}

		window.addEventListener("resize", updateWindowDimensions)
   		return () => window.removeEventListener("resize", updateWindowDimensions)
	}, [])

	const onSelectedTab = (index: number) => {
		const blocks = blocksRef?.current?.children
		
		if (blocks == undefined || blocksRef?.current == undefined)
			return

		if (blocks.length <= index)
			index = blocks.length - 1

		let totalOffset = 0
		for (let i = 0; i < index; i++) {
			const block = blocks[i]
			const width = block.getBoundingClientRect().width
			totalOffset += width
		}

		blocksRef.current!.style.left = `-${totalOffset}px`
	}

	return (
		<div className={styles.wrapper} ref={wrapperRef}>
			<BlockTabs 
				isVisible={isBlocksBeyond} 
				onSelected={onSelectedTab}
			/>
			<div className={styles.blocks} ref={blocksRef} data-beyond={isBlocksBeyond}>
				<InformationBlock />
				<BuysSellsBlock />
				<TransactionsBlock />
				<HoldersBlock />
				<FiltrationReportBlock />
			</div>
		</div>
	)
}

export default TokenBlocks
