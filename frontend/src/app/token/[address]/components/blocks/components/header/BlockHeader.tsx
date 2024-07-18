import styles from "./BlockHeader.module.scss"

type BlockHeaderProps = {
    children: any
}

const BlockHeader = ({children}: BlockHeaderProps) => {
  return (
    <div className={styles.header}>
      {children}
    </div>
  )
}

export default BlockHeader
