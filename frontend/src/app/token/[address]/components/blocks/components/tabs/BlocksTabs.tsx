import { cn } from "@/lib/utils"
import { useState } from "react"
import styles from "./BlockTabs.module.scss"

type BlockTabsProps = {
    isVisible: boolean
	onSelected: (index: number) => void
}

const tabs = [
  "Information",
  "Buys/Sells",
  "Transactions",
  "Holders",
  "Filtration report",
]

const BlockTabs = ({isVisible, onSelected}: BlockTabsProps) => {
	const [selectedIndex, setSelectedIndex] = useState(0)

	return (
		<div className={cn(styles.tabs)} data-visible={isVisible}>
		{tabs.map((o, i) => 
			<div 
				key={i} 
				className={cn(styles.tabs__item)} 
				data-selected={selectedIndex == i}
				onClick={() => {
					setSelectedIndex(i)
					onSelected(i)
				}}
			>
				{o}
			</div>
		)}
		</div>
	)
}

export default BlockTabs
