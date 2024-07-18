import { cn } from "@/lib/utils"
import styles from "./BaseBlockWrapper.module.scss"

type BaseBlockWrapperProps = {
  children?: any
  className?: string
}

const BaseBlockWrapper = ({children, className}: BaseBlockWrapperProps) => {
  return (
    <div className={cn(styles.block, className)}>
      {children}
    </div>
  )
}

export default BaseBlockWrapper
