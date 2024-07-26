import { TokenService } from "@/api/token.service"
import { Button } from "@/components/ui/button/Button"
import { ScrollArea } from "@/components/ui/scroll-area/ScrollArea"
import { useQuery } from "@/hooks/useQuery"
import { cn } from "@/lib/utils"
import { Token } from "@/types/Token"
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import { useEffect, useState } from "react"
import { CgMoreAlt } from "react-icons/cg"
import { RiLoader5Fill } from "react-icons/ri"
import TokenRow from "../token-row/TokenRow"
import styles from "./SidebarContent.module.scss"

const SidebarContent = () => {
	const {data: page, isLoading: pageIsLoading} = useQuery<{ items: Token[] }>({
		query: () => TokenService.GetTokensPage(1),
	})
	const [tokens, setTokens] = useState<Token[]>([])
	const [transaction, setTransaction] = useState<any>()

	useEffect(() => {
		if (page)
			setTokens(page.items)
	}, [page])

	useEffect(() => {
		if (!page)
			return

		const connection = new HubConnectionBuilder()
			.withUrl("http://localhost:5046/ws/new-tokens")
			.withAutomaticReconnect()
			.configureLogging(LogLevel.Information)
			.build()
	
		connection.on("ReceiveNewToken", (item: Token) => {
			connection.invoke("TrackTokenTransactions", item.address)
			setTokens(prevTokens => [item, ...prevTokens])
		});

		connection.on("ReceiveNewTransaction", (item) => {
			setTransaction(item)
		})

		connection.start()
	
		return () => {
			if (connection.state !== 'Disconnected') {
			connection.stop()
				.then(() => console.log("Disconnected from SignalR Hub"))
				.catch((err) => console.error("Error while disconnecting from SignalR Hub:", err))
			}
		};
	}, [page]);

	useEffect(() => {
		if (!transaction)
			return

		setTokens(prevTokens => prevTokens.map(o => {
			if (o.address == transaction.cryptoTokenAddress)
				o.priceUsd = transaction.type == 1 
					? transaction.boughtTokenPriceUsd
					: transaction.soldTokenPriceUsd
			return o
		}))
	}, [page, transaction])

  return (
    <ScrollArea className={styles.sidebar__content} scrollClassNames={styles["sidebar__content-scroll"]}>
        <table className={cn(styles.sidebar__table, styles.table)}>
			<thead>
				<tr className={styles.table__header}>
					<th></th>
					<th>Name</th>
					<th>MCap</th>
					<th>Created</th>
					<th></th>
				</tr>
			</thead>
            <tbody className={styles.table__content} data-loaded={!pageIsLoading}>
				{!pageIsLoading && tokens?.map((o, i) => <TokenRow key={o.address} {...o} />)}
            </tbody>
        </table>

		<div className={styles.loader} data-loaded={!pageIsLoading}>
			<RiLoader5Fill className={styles.loader__icon} />
		</div>

        {!pageIsLoading && <Button variant="ghost" className="mx-auto mt-3 mb-[300px]">
            <CgMoreAlt className="w-[16px] h-[16px]" />
            <span>more</span>
        </Button>}
    </ScrollArea>
  )
}

export default SidebarContent
