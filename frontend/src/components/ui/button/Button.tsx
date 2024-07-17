import { Slot } from "@radix-ui/react-slot"
import { cva, type VariantProps } from "class-variance-authority"
import * as React from "react"
import styles from "./Button.module.scss"

import { Tooltip, TooltipContent, TooltipTrigger } from "@/components/ui/tooltip/Tooltip"
import { cn } from "@/lib/utils"

const buttonVariants = cva(
  "",
  {
    variants: {
      variant: {
        ghost: styles.ghost,
        icon: styles.icon,
      },
      size: {
        default: styles.default_size,
        small: styles.small_size
      },
    },
    defaultVariants: {
      variant: "icon",
      size: "default",
    },
  }
)

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof buttonVariants> {
  asChild?: boolean
  tooltip?: string
  tooltipSide?: "right" | "left" | "top" | "bottom"
}

const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className, variant, size, asChild = false, tooltip, tooltipSide, ...props }, ref) => {
    const Comp = asChild ? Slot : "button"
    return (
      <Tooltip>
        <TooltipTrigger asChild>
          <Comp
            className={cn(buttonVariants({ variant, size, className }))}
            ref={ref}
            {...props}
          />
        </TooltipTrigger>
        {tooltip && <TooltipContent side={tooltipSide}>
            <p>{tooltip}</p>
        </TooltipContent>}
      </Tooltip>
    )
  }
)
Button.displayName = "Button"

export { Button, buttonVariants }
