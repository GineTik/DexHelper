import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar/Avatar"
import { Button } from "@/components/ui/button/Button"
import { Routes } from "@/lib/routes"
import { Token } from "@/types/Token"
import Link from "next/link"
import { useRouter } from "next/navigation"
import { BsThreeDotsVertical } from "react-icons/bs"
import styles from "./TokenRow.module.scss"
import MarketCapitalization from "./components/market-capitalization/MarketCapitalization"
import TokenLifeTime from "./components/token-life-time/TokenLifeTime"

type TokenRowProps = Token

const TokenRow = ({image, name, symbol, address: tokenAddress, createdAtUtc, priceUsd}: TokenRowProps) => {
	var router = useRouter()

	return (
		<tr className={styles.token} onClick={() => router.push(Routes.Common.Token(tokenAddress))}>
				<td>
					<div>
						<Avatar className={styles.token__image}>
							<AvatarImage src={image} alt={symbol} />
							<AvatarFallback>{symbol}</AvatarFallback>
						</Avatar>
					</div>
				</td>
				<td>
					<div className={styles.token__name}>{symbol}</div>
					<div className={styles.token__symbol}>{name}</div>
				</td>
				<td>
					<div className={styles.token__mcap}>
						<MarketCapitalization capitalization={priceUsd*1_000_000_000} />
					</div>
					<div className={styles['token__mcap-percent']}>+350%</div>
				</td>
				<td className={styles.token__created}>
					<TokenLifeTime dateUtc={createdAtUtc} />
				</td>
			<td className={styles.token__actions}>
				<Link href={`https://photon-sol.tinyastro.io/en/lp/${tokenAddress}`} target="_blank">
					<Button variant="icon" size="small" tooltip="to PhotonSol website" tooltipSide="right">
						<BsThreeDotsVertical className="text-white/50" />
					</Button>
				</Link>
			</td>
		</tr>
	)
}

export default TokenRow
