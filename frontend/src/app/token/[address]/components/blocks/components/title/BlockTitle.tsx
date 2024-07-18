import styles from "./BlockTitle.module.scss"

type BlockTitleProps = {
    children: string
}

const BlockTitle = ({children}: BlockTitleProps) => {
  return (
    <h2 className={styles.title}>
      {children}
    </h2>
  )
}

export default BlockTitle
