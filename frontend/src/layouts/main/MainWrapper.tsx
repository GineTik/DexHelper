import styles from "./MainWrapper.module.scss"

type MainWrapperProps = {
  children: any
}

const MainWrapper = ({children}: MainWrapperProps) => {
  return (
    <div className={styles.main}>
      {children}
    </div>
  )
}

export default MainWrapper
