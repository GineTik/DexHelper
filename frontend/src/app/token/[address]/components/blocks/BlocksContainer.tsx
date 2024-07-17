import styles from "./BlocksContainer.module.scss"
import InformationBlock from "./components/blocks/information/InformationBlock"
import TransactionsBlock from "./components/blocks/transactions/TransactionsBlock"

const BlocksContainer = () => {
  return (
    <div className={styles.blocks}>
      <InformationBlock />
      <TransactionsBlock />
    </div>
  )
}

export default BlocksContainer
