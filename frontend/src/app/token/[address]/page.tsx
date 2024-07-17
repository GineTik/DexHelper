import BlocksContainer from "./components/blocks/BlocksContainer"
import TokenChart from "./components/chart/TokenChart"
import TokenHeader from "./components/header/TokenHeader"
import styles from "./TokenPage.module.scss"

const TokenPage = () => {
  return (
    <div className={styles.page}>
      <TokenHeader />
      <TokenChart />
      <BlocksContainer />
    </div>
  )
}

export default TokenPage
