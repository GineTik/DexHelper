import TokenBlocks from "./components/blocks/TokenBlocks"
import TokenChart from "./components/chart/TokenChart"
import TokenHeader from "./components/header/TokenHeader"
import styles from "./TokenPage.module.scss"

const TokenPage = () => {
  return (
    <div className={styles.page}>
      <TokenHeader />
      <TokenChart />
      <TokenBlocks />
    </div>
  )
}

export default TokenPage
