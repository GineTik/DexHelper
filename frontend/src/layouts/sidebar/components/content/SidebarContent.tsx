import { Button } from "@/components/ui/button/Button"
import { ScrollArea } from "@/components/ui/scroll-area/ScrollArea"
import { cn } from "@/lib/utils"
import { Token } from "@/types/Token"
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import { useEffect, useState } from "react"
import { CgMoreAlt } from "react-icons/cg"
import TokenRow from "../token-row/TokenRow"
import styles from "./SidebarContent.module.scss"

const SidebarContent = () => {

	const [tokens, setTokens] = useState<Token[]>([])
	const [connection, setConnection] = useState<HubConnection | null>(null)
	const [transaction, setTransaction] = useState<any>()

	useEffect(() => {
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
  
	//   setConnection(connection);
  
	  return () => {
		if (connection.state !== 'Disconnected') {
		  connection.stop()
			.then(() => console.log("Disconnected from SignalR Hub"))
			.catch((err) => console.error("Error while disconnecting from SignalR Hub:", err))
		}
	  };
	}, []);

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
	}, [transaction])
  
	// useEffect(() => {
	//   if (connection) {
	// 	connection.start()
	// 	  .then(() => {
	// 		// connection.send("TestReceiveToken")
	// 		connection.stream("ReceiveNewTokens")
	// 			.subscribe({
	// 				next: (page) => {
	// 					const previousTokens = tokens
	// 					const intersectionIndex = page.items.indexOf(previousTokens[0])
	// 					const newTokens = previousTokens.slice(0, intersectionIndex)
				
	// 					console.log(previousTokens, )
	// 					for (var i = 0; i < newTokens.length; i++) {
	// 						const token = newTokens[i]
	// 						connection.send("TrackTokenTransactions", token.address)
	// 					}

	// 					setTokens(page.items)
	// 				},
	// 				error: (error) => {
	// 					console.log("Error: ", error)
	// 				},
	// 				complete: () => {
	// 					console.log("Completed")
	// 				}
	// 			})
	// 		console.log("Connected to SignalR Hub")
	// 	  })
	// 	  .catch((err) => {
	// 		console.error("Error while connecting to SignalR Hub:", err)
	// 	  })
	//   }
	// }, [connection])

  return (
    <ScrollArea className={styles.sidebar__content} scrollClassNames={styles["sidebar__content-scroll"]}>
        <table className={cn(styles.sidebar__table, styles.table)}>
            <tbody>
				<tr className={styles.table__header}>
					<th></th>
					<th>Name</th>
					<th>MCap</th>
					<th>Created</th>
					<th></th>
				</tr>

				{tokens?.map((o, i) => <TokenRow key={i} {...o} />)}

            </tbody>
        </table>
        <Button variant="ghost" className="mx-auto mt-3 mb-[300px]">
            <CgMoreAlt className="w-[16px] h-[16px]" />
            <span>more</span>
        </Button>
    </ScrollArea>
  )
}

export default SidebarContent
