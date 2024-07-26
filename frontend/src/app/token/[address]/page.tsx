"use client"
import { TokenService } from "@/api/token.service"
import { useQuery } from "@/hooks/useQuery"
import { useParams } from "next/navigation"
import TokenBlocks from "./components/blocks/TokenBlocks"
import TokenChart from "./components/chart/TokenChart"
import TokenHeader from "./components/header/TokenHeader"
import styles from "./TokenPage.module.scss"

const TokenPage = () => {
  const {address} = useParams()

  const {data, isLoading} = useQuery({
    query: () => TokenService.GetTokenInformation(address as string)
  })

  if (isLoading)
    return "is loading ..."

  return (
    <div className={styles.page}>
      <TokenHeader {...data} />
      <TokenChart symbol={data.symbol} />
      <TokenBlocks />
    </div>
  )
}

export default TokenPage
