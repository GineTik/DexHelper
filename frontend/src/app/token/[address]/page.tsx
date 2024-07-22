"use client"
import axios from "axios"
import { useParams } from "next/navigation"
import { useEffect, useState } from "react"
import TokenBlocks from "./components/blocks/TokenBlocks"
import TokenChart from "./components/chart/TokenChart"
import TokenHeader from "./components/header/TokenHeader"
import styles from "./TokenPage.module.scss"

const TokenPage = () => {
  const {address} = useParams()
  const [data, setData] = useState<any>({})
  
  useEffect(() => {
    axios.get(`http://localhost:5046/tokens/get-info?address=${address}`)
      .then(o => setData(o.data))
  }, [])

  return (
    <div className={styles.page}>
      <TokenHeader {...data} />
      <TokenChart symbol={data.symbol} />
      <TokenBlocks />
    </div>
  )
}

export default TokenPage
